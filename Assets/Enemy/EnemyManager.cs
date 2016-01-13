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
    enum State
    {
        None,
        Start,      //< 待機前
        Standby,    //< 待機
        Update,     //< アップデート
    }

    [SerializeField]
    float delayTime = 2.0f;

    [SerializeField]
    SpriteRenderer enemyRenderer = null;

    [SerializeField]
    List<EnemyData> enemyList = new List<EnemyData>();  //< 登録エネミー

    int activeEnemyID = 0;  //< 出現するエネミーID

    State state = State.None;    //< 制御する状態

    void Awake()
    {
        base.Awake();
        
    }

    // Use this for initialization
    void Start()
    {
        base.Start();

        state = State.Start;

    }

    /// <summary>
    /// 敵の画像情報を設定する。
    /// </summary>
    /// <param name="sprite"></param>
    public void SetEnemySprite(Sprite sprite)
    {
        enemyRenderer.sprite = sprite;
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


    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (!Vuforia.VuforiaBehaviour.IsMarkerLookAt) return;
        
        switch (state)
        {
            case State.Start:    //< 待機前処理

                // サバ―にエネミーを登録,初期化を行う
                GetActiveEnemyData().SetMyDate();

                Debugger.Log("登録終了");

                GetActiveEnemyData().StateChange(EnemyData.EnamyState.STAY);

                Debugger.Log(">> GetActiveEnemy State STAY");

                state = State.Standby;
                break;

            case State.Standby:  //< 待機　出現するかを制御
                GetActiveEnemyData().StateChange(EnemyData.EnamyState.SPAWN);

                Debugger.Log(">> GetActiveEnemy State SPAWN");

                state = State.Update;
                break;

            case State.Update:   //< アップデート処理

                // 死んでないなら処理をする。
                if (GetActiveEnemyData().State != EnemyData.EnamyState.DEAD)
                {
                    GetActiveEnemyData().UpdateData();

                    // SV側がHitフラグだ立ったら、CL状態を変更する。
                    if (GetActiveEnemyData().IsHit())
                    {
                        GetActiveEnemyData().StateChange(EnemyData.EnamyState.HIT);

                        SEPlayer.Instance.Play("EnemyHit");
                       
                        Debugger.Log(">> GetActiveEnemy State HIT");
                    }

                    // SV側のライフが0なら、CL状態を変更する。
                    if (GetActiveEnemyData().Life <= 0)
                    {
                        GetActiveEnemyData().HitRelease();
                        GetActiveEnemyData().StateChange(EnemyData.EnamyState.DEAD);

                        SEPlayer.Instance.Play("EnemySiren");

                        Debugger.Log(">> GetActiveEnemy State DEAD");
                    }
                }
                else
                {
                    // SV側がアクティブでないなら、登録してある次のエネミーを設定する。
                    if (!GetActiveEnemyData().IsActive())
                    {
                        StartCoroutine("WaitNextWave");
                    }
                }

                break;

        }

    }


    IEnumerator WaitNextWave()
    {
        yield return new WaitForSeconds(delayTime);

        activeEnemyID++;

        if (activeEnemyID >= enemyList.Count)
        {
            SequenceManager.Instance.ChangeScene(SceneID.RESULT);
            yield return null;
        }

        SEPlayer.Instance.Play("EnemyAppearance");

        state = State.Start;
        Debugger.Log(">> 次のWaveに遷移する");

    }
}
