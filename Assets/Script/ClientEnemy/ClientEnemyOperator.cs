//-------------------------------------------------------------
// データのやり取りして、エネミーを操作するクラス
//
// code by m_yamada featuring Gai
//-------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class ClientEnemyOperator : MonoBehaviour 
{

    [SerializeField]
    float attackTiming = 2.0f;

    SpriteRenderer spriteRenderer = null;

    bool isLive = false;
    bool isHit = false;
    float attackTime = 0;

    Animation animationAI = null;
    AnimationState animationState = null;
    float animationTime = 0;
    float pauseAnimationTime = 0;


    // Use this for initialization
    void Start()
    {
        animationAI = GetComponent<Animation>();
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
            attackTime = attackTiming;
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Vuforia.VuforiaBehaviour.IsMarkerLookAt) return;
        
        // プレイヤーの方向に向く
        transform.LookAt(new Vector3(GameManager.Instance.GetPlayerData().Position.x, 
            transform.position.y, GameManager.Instance.GetPlayerData().Position.z));

        switch (EnemyManager.Instance.GetActiveEnemyData().State)
        {
            case EnemyData.EnamyState.ACTIVE:
                if (TutorialScript.IsTutorial) break;

                animationTime = animationState.normalizedTime;

                spriteRenderer.color = EnemyManager.Instance.SpriteColor;
                spriteRenderer.sprite = EnemyManager.Instance.GetStandingSpriteAutoAnim();

                if (IsAttackTiming())
                {
                    EnemyManager.Instance.AttackSpriteAnimPlay();
                    EnemyManager.Instance.GetActiveEnemyData().StateChange(EnemyData.EnamyState.ATTACK);

                    AIAnimationPause();
                }

                break;
            case EnemyData.EnamyState.ATTACK:
                spriteRenderer.sprite = EnemyManager.Instance.GetAttackSpriteAutoAnim();

                if (!EnemyManager.Instance.IsAttackAnimPlay())
                {
                    EnemyManager.Instance.StandingSpriteAnimPlay();
                    EnemyAttackManager.Instance.CreateAttack(transform.position - new Vector3(0, 200, 0));
                    EnemyManager.Instance.GetActiveEnemyData().StateChange(EnemyData.EnamyState.ACTIVE);

                    AttackSoundPlay();
                    AIAnimationPlay();
                }
                break;

            case EnemyData.EnamyState.HIT:
                Hit();

                if (!animationAI.isPlaying)
                {
                    animationAI.Stop();
                    HitEffectCompleted();
                }

                break;

            case EnemyData.EnamyState.SPAWN:
                Spawn();

                if (!animationAI.isPlaying)
                {
                    animationAI.Stop();
                    SpawnCompleted();
                }
                break;

            case EnemyData.EnamyState.STAY:
                break;

            case EnemyData.EnamyState.DEAD:
                Dead();
                break;
        }

    }

    /// <summary>
    /// 発生
    /// </summary>
    void Spawn()
    {
        if (isLive) return;

        isLive = true;

        // 初期化
        isHit = false;
        attackTime = 0;

        EnemyManager.Instance.StandingSpriteAnimPlay();

        spriteRenderer.sprite = EnemyManager.Instance.GetStandingSpriteAutoAnim();
        spriteRenderer.color = EnemyManager.Instance.SpriteColor;

        ChangeActive();

        Debugger.Log(">> Spawn()");

        SpawnAnimationPlay();

        attackTime = attackTiming;
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

        SEPlayer.Instance.Play(Audio.SEID.ENEMYSIREN);
    }

    /// <summary>
    /// 衝突時の処理
    /// </summary>
    void Hit()
    {
        if (isHit) return;

        isHit = true;
        AIAnimationPause();

        animationAI.PlayQueued("anim_enemy_hit");

        // HitEffect再生 座標の-30は、敵の手前に出す数値
        HitEffectManager.Instance.EffectPlay(
            EnemyManager.Instance.GetActiveEnemyData().HitSkillType(), 
            transform.position - new Vector3(0,0,30));

        SEPlayer.Instance.Play(Audio.SEID.ENEMYHIT);
    }


    void HitEffectCompleted()
    {
        AIAnimationPlay();

        isHit = false;
        iTween.Stop(gameObject);
        EnemyManager.Instance.GetActiveEnemyData().StateChange(EnemyData.EnamyState.ACTIVE);
    }

    /// <summary>
    /// アクティブ状態を変更する
    /// </summary>
    void ChangeActive()
    {
        GameManager.Instance.SendEnemyIsActive(EnemyManager.Instance.GetActiveEnemyData().Id, isLive);
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


    // ---------------------------------------------------------
    //  アニメーション系
    // --------------------------------------------------------

    void AIAnimationStop()
    {
        animationTime = 0;
        pauseAnimationTime = 0;
        animationState = null;
        animationAI.Stop();
    }

    void AIAnimationPause()
    {
        pauseAnimationTime = animationTime;
        animationAI.Stop();
    }

    void AIAnimationPlay()
    {
        switch (EnemyManager.Instance.GetActiveEnemyData().EnemyType)
        {
            case EnemyMasterData.ENEMY_TYPE.GOREMU:
                animationState = animationAI.PlayQueued("anim_golem_move");
                break;
            case EnemyMasterData.ENEMY_TYPE.SMALL_DORAGON:
                animationState = animationAI.PlayQueued("anim_small_dragon_move");
                break;
            case EnemyMasterData.ENEMY_TYPE.BIG_DORAGON:
                animationState = animationAI.PlayQueued("anim_big_dragon_move");
                break;
        }

        animationState.normalizedTime = pauseAnimationTime;
    }

    void SpawnAnimationPlay()
    {
        switch (EnemyManager.Instance.GetActiveEnemyData().EnemyType)
        {
            case EnemyMasterData.ENEMY_TYPE.GOREMU:
                animationAI.PlayQueued("anim_golem_spawn");
                break;
            case EnemyMasterData.ENEMY_TYPE.SMALL_DORAGON:
                animationAI.PlayQueued("anim_small_dragon_spawn");
                break;
            case EnemyMasterData.ENEMY_TYPE.BIG_DORAGON:
                animationAI.PlayQueued("anim_big_dragon_spawn");
                break;
        }
    }
}
