//-------------------------------------------------------------
//  ゲーム管理クラス
//  データの管理、同期を行います 
//
//  code by m_yamada
//  and ogata
//  and Keijiro Sugimoto
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    /// <summary>
    /// プレイヤーを登録
    /// </summary>
    [SerializeField]
    GameObject player = null;

    /// <summary>
    /// 攻撃エフェクトのターゲットを登録
    /// </summary>
    [SerializeField]
    GameObject enemy = null;

    public GameObject Player { get { return player; } }
    public GameObject Enemy { get { return enemy; } }


    //フォトン用view
    PhotonView view = null;

    // プレイヤーの体力の初期値
    [SerializeField]
    int PLAYER_INIT_HP = 100;

    // プレイヤーデータ
    const int MAXIMUM_PLAYER_NUM = 1;   // 最大数　プレイヤー
    PlayerMasterData playerData = new PlayerMasterData();

    // エネミーデータ
    const int MAXIMUM_ENEMY_NUM = 1;    // 最大数　エネミー
    EnemyMasterData enemyDataArray = new EnemyMasterData();

    [SerializeField]
    Text debugPlayerPositionText = null;

    //初回のみの初期化処理
    public override void Awake()
    {
        base.Awake();

        view = GetComponent<PhotonView>();


        // ARCameraが入っているか（クライアントのみの処理）
        if (ConnectionManager.IsSmartPhone) {
            if (SequenceManager.Instance.ARCamera == null)
            {
                Debugger.LogError("GameManagerのARCameraがnullです！！");
            }
        }

        SendPlayerDataAwake();
        SendEnemyDataAwake();
    }

    //毎回の初期化処理
    public override void Start()
    {
        base.Start();



    }

    //更新
    public override void Update()
    {
        base.Update();

        UpdateClient();
        
    }

    //クライアントのみ行うアップデートです。
    private void UpdateClient()
    {
        TimeUIInfo.UpdateTimeUI();

        // タイムオーバー処理をここに記述する。
        if (TimeUIInfo.IsTimeOver())
        { 
            
        }

#if !UNITY_EDITOR
        //クライアント確認
        if(!ConnectionManager.IsSmartPhone){
            return;
        }
#endif

        //スマフォ位置更新
        UpdateClientPosition();

    }


    /// <summary>
    /// スマートフォンの位置情報を更新します。
    /// </summary>
    private void UpdateClientPosition()
    {
#if !UNITY_EDITOR
        // ターゲット画像が読み込まれていないなら処理しない。
        if (!Vuforia.VuforiaBehaviour.IsMarkerLookAt)
        {
           // Debugger.Log("ターゲットロスト");
            return;
        }
#endif

        //  スマートフォンのポジションの同期
#if UNITY_EDITOR
        playerData.Position = SequenceManager.Instance.SingleCamera.transform.position ;
#else
        playerData.Position = SequenceManager.Instance.ARCamera.transform.position ;
#endif

        player.transform.position  = playerData.Position;
        debugPlayerPositionText.text = playerData.Position.ToString();
    }

    /// <summary>
    /// プレイヤー情報の初回初期化
    /// </summary>
    void SendPlayerDataAwake()
    {
        playerData = new PlayerMasterData();
        playerData.HelthPoint = PLAYER_INIT_HP;
    }


    /// <summary>
    /// 敵情報の初回初期化
    /// </summary>
    void SendEnemyDataAwake()
    {
        enemyDataArray = new EnemyMasterData();
    }



    /// <summary>
    /// プレイヤーの情報を取得したい場合、この関数を呼んでください。
    /// ※取得したデータを直接書き換えないでください。
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public PlayerMasterData GetPlayerData()
    {
        return playerData;
    }

    /// <summary>
    /// エネミーの情報を取得したい場合、この関数を読んでください、
    /// ※取得したデータを直接書き換えないでください。
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public EnemyMasterData GetEnemyData()
    {   
        return enemyDataArray;
    }



    /// <summary>
    /// エネミーのタイプを変更します。
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_isActive"></param>
    public void SendEnemyType(EnemyMasterData.ENEMY_TYPE _type)
    {
        enemyDataArray.EnemyType = _type;
    }



    /// <summary>
    /// エネミーの生存フラグを変更します。
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_isActive"></param>
    public void SendEnemyIsActive(bool _isActive)
    {
        enemyDataArray.IsActive = _isActive;
    }

    /// <summary>
    /// エネミーのHPをセット(変更)します。（同期）
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_hp"></param>
    public void SendEnemyHP(int _hp)
    {
        enemyDataArray.HP = _hp;
    }

    
    /// <summary>
    /// エネミーの座標を変更します。（同期）
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_pos"></param>
    public void SendEnemyPosition(Vector3 _pos)
    {
        enemyDataArray.Position = _pos;
    }


    /// <summary>
    /// エネミーのローテーションを変更します。（同期）
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_pos"></param>
    public void SendEnemyRotation(Vector3 _rotation)
    {
        enemyDataArray.Rotation = _rotation;
    }

    /// <summary>
    /// プレイヤーのヒットフラグを処理する。
    /// </summary>
    /// <param name="isHit"></param>
    public void SendPlayerHit(bool isHit,int damage)
    {
        playerData.IsHit = isHit;
        playerData.HelthPoint -= damage;
        if (playerData.HelthPoint <= 0){
            //死んだ処理の記述予定
        }
        else{
            Debugger.Log("Player HP = " + playerData.HelthPoint);
        }

        Debugger.Log("プレイヤーヒット");
    }

    /// <summary>
    /// プレイヤーの座標を変更する関数です。（同期）
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_pos"></param>
    public void SendPlayerPosition( Vector3 _pos)
    {
        playerData.Position = _pos;
    }

    
    /// <summary>
    /// エネミーが攻撃に当たった時用の関数
    /// </summary>
    /// <param name="_playerAttackArrayNumber"></param>
    /// <param name="_damage"></param>
    public void SendEnemyHit(MotionManager.MotionSkillType _playerAttackType,int _damage)
    {
        enemyDataArray.HP -= _damage;
        enemyDataArray.HitAttackType = _playerAttackType;
        enemyDataArray.IsHit = true;
    }

    /// <summary>
    /// リザルトシーンに遷移
    /// </summary>
    public void ChangeResultScene()
    {
        view.RPC("SyncChangeResultScene", PhotonTargets.All);
    }

    private void SyncChangeResultScene(PhotonMessageInfo info)
    {
        SequenceManager.Instance.ChangeScene(SceneID.RESULT);
    }

    

}
