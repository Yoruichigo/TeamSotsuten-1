//-------------------------------------------------------------
// データのやり取りして、エネミーを操作するクラス
//
// code by m_yamada featuring Gai
//-------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class ClientEnemyOperator : MonoBehaviour 
{

    SpriteRenderer spriteRenderer = null;

    bool isLive = false;
    bool isHit = false;
    float attackTime = 0;

    Animation animationAI = null;
    AnimationState animationState = null;
    float animationTime = 0;
    float pauseAnimationTime = 0;
    float voicePlayTiming = 0;

    bool spwanAnimHandle = false;
    bool hitAnimHandle = false;
    bool isAnimPause = false;

    // Use this for initialization
    void Start()
    {
        animationAI = transform.parent.GetComponent<Animation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    /// <summary>
    /// 攻撃のタイミングかどうか
    /// </summary>
    /// <returns></returns>
    bool IsAttackTiming()
    {
        attackTime -= Time.deltaTime;
        if (attackTime <= 0)
        {
            attackTime = EnemyManager.Instance.GetActiveEnemyData().AttackTiming ;
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR
        if (!Vuforia.VuforiaBehaviour.IsMarkerLookAt) return;
#endif

        // もしエネミーがいないなら処理しない
        if (EnemyManager.Instance.IsEnemyNothing) return;
        if (TutorialSequence.IsTutorial) return;

        switch (EnemyManager.Instance.GetActiveEnemyData().State)
        {
            case EnemyData.EnamyState.ACTIVE:

                animationTime = animationState.normalizedTime;
                spriteRenderer.sprite = EnemyManager.Instance.GetStandingSpriteAutoAnim();
                spriteRenderer.color = EnemyManager.Instance.SpriteColor;
                
                voicePlayTiming -= Time.deltaTime;
                if (voicePlayTiming <= 0)
                {
                    voicePlayTiming = Random.Range(3.0f,10.0f);
                    VoiceSoundPlay();
                }

                if (IsAttackTiming())
                {
                    EnemyManager.Instance.AttackSpriteAnimPlay();
                    EnemyManager.Instance.GetActiveEnemyData().StateChange(EnemyData.EnamyState.ATTACK);

                    AIAnimationPause();
                }

                break;
            case EnemyData.EnamyState.ATTACK:
                spriteRenderer.sprite = EnemyManager.Instance.GetAttackSpriteAutoAnim();
                spriteRenderer.color = EnemyManager.Instance.SpriteColor;

                if (!EnemyManager.Instance.IsAttackAnimPlay())
                {
                    EnemyManager.Instance.StandingSpriteAnimPlay();

                    var enemyData = EnemyManager.Instance.GetActiveEnemyData();

                    EnemyAttackManager.Instance.CreateAttack(transform.position, enemyData.AttackPower);
                    enemyData.StateChange(EnemyData.EnamyState.ACTIVE);

                    AttackSoundPlay();
                    AIAnimationPlay();
                }

                break;

            case EnemyData.EnamyState.HIT:

                if (hitAnimHandle && !animationAI.isPlaying)
                {
                    animationAI.Stop();
                    hitAnimHandle = false;
                    HitEffectCompleted();
                    return;
                }

                Hit();

                break;

            case EnemyData.EnamyState.SPAWN:

                if (spwanAnimHandle && !animationAI.isPlaying)
                {
                    animationAI.Stop();
                    spwanAnimHandle = false;
                    SpawnCompleted();
                    return;
                }

                Spawn();

                break;

            case EnemyData.EnamyState.STAY:
                break;

            case EnemyData.EnamyState.DEAD:
                Dead();
                break;
        }

    }

    AnimationClip animAIClip = null;
    AnimationClip animSpwanClip = null;

    // 初期化
    void Init()
    {
        isHit = false;
        attackTime = 0;
        hitAnimHandle = false;
        spwanAnimHandle = false;
        transform.localPosition = Vector3.zero;
        transform.parent.position = Vector3.zero;
        voicePlayTiming = 0;

        animAIClip = EnemyManager.Instance.GetActiveEnemyData().AnimationAIClip;
        animSpwanClip = EnemyManager.Instance.GetActiveEnemyData().AnimationSpwanClip;

        EnemyManager.Instance.StandingSpriteAnimPlay();

        spriteRenderer.sprite = EnemyManager.Instance.GetStandingSpriteAutoAnim();
        spriteRenderer.color = EnemyManager.Instance.SpriteColor;

    }

    /// <summary>
    /// 発生
    /// </summary>
    void Spawn()
    {
        if (isLive) return;

        isLive = true;

        Init();

        ChangeActive();

        animationAI.PlayQueued(animSpwanClip.name);
        spwanAnimHandle = true;

        attackTime = EnemyManager.Instance.GetActiveEnemyData().AttackTiming;

        Debugger.Log(">> Spawn()");

    }

    void SpawnCompleted()
    {
        EnemyManager.Instance.GetActiveEnemyData().StateChange(EnemyData.EnamyState.ACTIVE);

        AIAnimationPlay();
    }

    /// <summary>
    /// 死亡
    /// </summary>
    void Dead()
    {
        if (!isLive) return;

        AIAnimationStop();

        isLive = false;
        ChangeActive();

        SEPlayer.Instance.Play(Audio.SEID.EXPLOSION_1);
    }

    /// <summary>
    /// 衝突時の処理
    /// </summary>
    void Hit()
    {
        if (isHit) return;

        isHit = true;

        if (!hitAnimHandle)
        {
            AIAnimationPause();
            animationAI.PlayQueued("anim_enemy_hit");
        }

        hitAnimHandle = true;

        // HitEffect再生 座標の-30は、敵の手前に出す数値
        HitEffectManager.Instance.EffectPlay(
            EnemyManager.Instance.GetActiveEnemyData().HitSkillType(), 
            transform.position - new Vector3(0,0,300));

        SEPlayer.Instance.Play(Audio.SEID.ENEMYHIT);
    }


    void HitEffectCompleted()
    {
        AIAnimationPlay();

        isHit = false;
        EnemyManager.Instance.GetActiveEnemyData().StateChange(EnemyData.EnamyState.ACTIVE);
        
        EnemyManager.Instance.StandingSpriteAnimPlay();

        EnemyManager.Instance.GetActiveEnemyData().HitRelease();
    }

    /// <summary>
    /// アクティブ状態を変更する
    /// </summary>
    void ChangeActive()
    {
        GameManager.Instance.SendEnemyIsActive(isLive);
    }


    // ---------------------------------------------
    // サウンド系
    // ---------------------------------------------

    /// <summary>
    /// 種類別に攻撃のサウンドを再生する。
    /// </summary>
    void AttackSoundPlay()
    {
        switch (EnemyManager.Instance.GetActiveEnemyData().EnemyType)
        {
            case EnemyMasterData.ENEMY_TYPE.GOREMU:
                SEPlayer.Instance.Play(Audio.SEID.GOLEMATTACK);
                break;
            case EnemyMasterData.ENEMY_TYPE.SMALL_DORAGON:
                SEPlayer.Instance.Play(Audio.SEID.GOLEMATTACK);
                break;
            case EnemyMasterData.ENEMY_TYPE.BIG_DORAGON:
                SEPlayer.Instance.Play(Audio.SEID.GOLEMATTACK);
                break;
        }
    }

    /// <summary>
    /// 種類別に攻撃のサウンドを再生する。
    /// </summary>
    void VoiceSoundPlay()
    {
        switch (EnemyManager.Instance.GetActiveEnemyData().EnemyType)
        {
            case EnemyMasterData.ENEMY_TYPE.GOREMU:
                SEPlayer.Instance.Play(Audio.SEID.DRAGONSIGH);
                break;
            case EnemyMasterData.ENEMY_TYPE.SMALL_DORAGON:
                SEPlayer.Instance.Play(Audio.SEID.DRAGONROAR);
                break;
            case EnemyMasterData.ENEMY_TYPE.BIG_DORAGON:
                SEPlayer.Instance.Play(Audio.SEID.DRAGONROAR);
                break;
        }
    }



    // ---------------------------------------------------------
    //  アニメーション系
    // --------------------------------------------------------

    void AIAnimationStop()
    {
        animationTime = 0;
        pauseAnimationTime = 0;
        animationState = null;
        animationAI.Stop();

        isAnimPause = false;
    }

    void AIAnimationPause()
    {
        if (!isAnimPause)
        {
            pauseAnimationTime = animationTime;
            isAnimPause = true;
        }

        animationAI.Stop();
    }

    void AIAnimationPlay()
    {
        isAnimPause = false;
        animationState = animationAI.PlayQueued(animAIClip.name);
        animationState.normalizedTime = pauseAnimationTime;
    }
}
