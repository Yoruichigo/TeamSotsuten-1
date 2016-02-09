/// ----------------------------------------
/// クライアントエネミーを制御する
///
/// code by m_yamada
/// ----------------------------------------


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : Singleton<EnemyManager>
{
    class ClientEnemyData
    {
        public Transform trans = null;
        public SpriteRenderer sprite = null;
        public Canvas canvas = null;
    }

    enum State
    {
        None,
        Start,      //< 待機前
        Standby,    //< 待機
        Update,     //< アップデート
    }

    [SerializeField]
    float changeSpriteAnimTime = 1.0f;

    [SerializeField]
    float nextWaveTime = 2.0f;

    [SerializeField]
    Transform clientEnemy = null;

    [SerializeField]
    EnemyHelthVar enemyHelthVar = null;

    [SerializeField]
    Transform appearanceEffectRoot = null;

    [SerializeField]
    Transform destroyEffectRoot = null;

    [HideInInspector]
    public Color SpriteColor = Color.white;

    List<EnemyData> enemyList = new List<EnemyData>();  //< 登録エネミー

    ClientEnemyData clientEnemyData = new ClientEnemyData();

    Sprite[] standingSpriteList = null;
    Sprite[] attackSpriteList = null;

    ParticleSystem appearanceEffect = null; //< 登場するエフェクト
    ParticleSystem destroyEffect = null;    //< 死亡するエフェクト
    State state = State.None;    //< 制御する状態

    bool isStandingAnimPlay = false;
    bool isAttackAnimPlay = false;

    int activeEnemyID = 0;  //< 出現するエネミーID
    int standingSpriteIndex = 0;
    int attackSpriteIndex = 0;

    float delayTime = 0;
    float animTime = 0;

    public bool IsEnemyNothing { get; private set; }

    public override void Awake()
    {
        base.Awake();
        delayTime = nextWaveTime;

        for (int i = 0; i < transform.childCount; i++)
        {
            enemyList.Add(transform.GetChild(i).GetComponent<EnemyData>());
        }

        clientEnemyData.trans = clientEnemy;
        clientEnemyData.sprite = clientEnemyData.trans.GetComponentInChildren<SpriteRenderer>();
        clientEnemyData.canvas = clientEnemyData.trans.GetComponentInChildren<Canvas>();
    }

    public override void Start()
    {
        base.Start();

        state = State.Start;

        appearanceEffect = appearanceEffectRoot.GetChild(0).GetComponent<ParticleSystem>();
        destroyEffect = destroyEffectRoot.GetChild(0).GetComponent<ParticleSystem>();

        appearanceEffect.Stop();
        destroyEffect.Stop();

        EnemyShowEnable(false);
    }


    /// <summary>
    /// 敵の画像情報を設定する。
    /// </summary>
    /// <param name="sprite"></param>
    public void SetEnemySprite(ref Sprite[] standingSpriteArray,ref Sprite[] attackSpriteArray,ref Color color)
    {
        standingSpriteList = standingSpriteArray;
        attackSpriteList = attackSpriteArray;
        SpriteColor = color;
    }

    /// <summary>
    /// 現在アクティブなエネミーのデータを取得
    /// </summary>
    /// <returns></returns>
    public EnemyData GetActiveEnemyData()
    {
        if (activeEnemyID >= enemyList.Count)
        {
            // 登録してある数を超えた場合は、returnで処理させない。
            return null;
        }

        return enemyList[activeEnemyID];
    }

    public override void Update()
    {
        base.Update();

        if (TutorialScript.IsTutorial) return;

#if !UNITY_EDITOR
        if (!Vuforia.VuforiaBehaviour.IsMarkerLookAt) return;
#endif
        if (IsEnemyNothing) return;

        switch (state)
        {
            case State.Start:    //< 待機前処理

                appearanceEffect.Play();

                // サバ―にエネミーを登録,初期化を行う
                GetActiveEnemyData().SetMyDate();
                enemyHelthVar.SetMaxLife(GetActiveEnemyData().Life);

                Debugger.Log("登録終了");

                GetActiveEnemyData().StateChange(EnemyData.EnamyState.STAY);

                Debugger.Log(">> GetActiveEnemy State STAY");

                state = State.Standby;
                break;

            case State.Standby:  //< 待機　出現するかを制御
                GetActiveEnemyData().StateChange(EnemyData.EnamyState.SPAWN);

                StandingSpriteAnimPlay();

                Debugger.Log(">> GetActiveEnemy State SPAWN");

                state = State.Update;
                break;

            case State.Update:   //< アップデート処理
                
                // 死んでないなら処理をする。
                if (GetActiveEnemyData().State != EnemyData.EnamyState.DEAD)
                {
                    bool isEnable = true;
#if UNITY_EDITOR

#else 
                isEnable = Vuforia.VuforiaBehaviour.IsMarkerLookAt;
#endif
                    EnemyShowEnable(isEnable);

                    GetActiveEnemyData().UpdateData();

                    // SV側のライフが0なら、CL状態を変更する。
                    if (GetActiveEnemyData().Life < 0)
                    {
                        GetActiveEnemyData().HitRelease();
                        GetActiveEnemyData().StateChange(EnemyData.EnamyState.DEAD);

                        EnemyShowEnable(false);

                        destroyEffect.transform.position = clientEnemyData.trans.position;
                        destroyEffect.Play();

                        Debugger.Log(">> GetActiveEnemy State DEAD");
                        break;
                    }

                    // SV側がHitフラグだ立ったら、CL状態を変更する。
                    if (GetActiveEnemyData().IsHit())
                    {
                        GetActiveEnemyData().HitRelease();

                        // 除外する状態
                        if (GetActiveEnemyData().State == EnemyData.EnamyState.ATTACK) break;
                        if (GetActiveEnemyData().State == EnemyData.EnamyState.SPAWN) break;
                        
                        GetActiveEnemyData().StateChange(EnemyData.EnamyState.HIT);
                        Debugger.Log(">> GetActiveEnemy State HIT");

                        break;
                    }

                }
                else
                {
                    GetActiveEnemyData().HitRelease();
                    NextWave();
                }

                break;

        }

    }


    void EnemyShowEnable(bool isEnable)
    {
        clientEnemyData.canvas.enabled = isEnable;
        clientEnemyData.sprite.enabled = isEnable;
    }


    void NextWave()
    {
        delayTime -= Time.deltaTime;
        if (delayTime > 0) return;

        delayTime = nextWaveTime;

        activeEnemyID++;

        if (activeEnemyID >= enemyList.Count)
        {
            IsEnemyNothing = true;
            return;
        }

        SEPlayer.Instance.Play(Audio.SEID.ENEMYAPPEARANCE);

        state = State.Start;
        Debugger.Log(">> 次のWaveに遷移する");
    }

    // --------------------------------------------------------------------
    // アニメーション系
    // ---------------------------------------------------------------------

    // 立ちアニメーション再生中かどうか
    public bool IsStandingAnimPlay()
    {
        return isStandingAnimPlay;
    }

    // 攻撃アニメーション再生中かどうか
    public bool IsAttackAnimPlay()
    {
        return isAttackAnimPlay;
    }

    // 立ちアニメーションを再生
    public void StandingSpriteAnimPlay()
    {
        animTime = 0;
        standingSpriteIndex = 0;
        isStandingAnimPlay = true;
        isAttackAnimPlay = false;
    }

    // 攻撃アニメーションを再生
    public void AttackSpriteAnimPlay()
    {
        animTime = 0;
        attackSpriteIndex = 0;
        isAttackAnimPlay = true;
        isStandingAnimPlay = false;
    }

    // 立っている状態の画像を取得
    public Sprite GetStandingSpriteAutoAnim()
    {
        if (!isStandingAnimPlay) return standingSpriteList[0];

        animTime += Time.deltaTime;
        if (animTime >= changeSpriteAnimTime)
        {
            animTime = 0;

            standingSpriteIndex++;
            if (standingSpriteIndex >= GetStandingSpriteLength())
            {
                // loopなので、コメントにする
                //isStandingAnimPlay = false;
                standingSpriteIndex = 0;
            }
        }

        return standingSpriteList[standingSpriteIndex];
    }

    // 攻撃状態の画像を取得
    public Sprite GetAttackSpriteAutoAnim()
    {
        if (!isAttackAnimPlay) return attackSpriteList[0];

        animTime += Time.deltaTime;
        if (animTime >= changeSpriteAnimTime)
        {
            animTime = 0;

            attackSpriteIndex++;
            if (attackSpriteIndex >= GetAttackSpriteLength())
            {
                isAttackAnimPlay = false;
                attackSpriteIndex = 0;
            }
        }

        return attackSpriteList[attackSpriteIndex];
    }

    // 立っている状態の画像の長さを取得
    public int GetStandingSpriteLength()
    {
        return standingSpriteList.Length;
    }

    // 攻撃状態の画像の長さを取得
    public int GetAttackSpriteLength()
    {
        return attackSpriteList.Length;
    }
}
