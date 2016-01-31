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

    // 時間表示
    [System.Serializable]
    public struct TimeCounter
    {
        /// <summary>
        /// ゲーム開始時からの経過時間
        /// </summary>
        public float deltaSecond;

        /// <summary>
        /// 最大時間
        /// </summary>
        public float maxSecond;

        /// <summary>
        /// タイムオーバーをしているかどうか
        /// true...制限時間を過ぎている。 false...まだ制限時間内
        /// </summary>
        public bool isTimeOver;

        /// <summary>
        /// 時間を表示するオブジェクトのタグ
        /// </summary>
        public string timeTextObjectTag;
    
    }

    [SerializeField]
    TimeCounter timeCounter;

    [SerializeField]
    Text debugPlayerPositionText = null;

    /// <summary>
    /// タイム表示用テキスト
    /// </summary>
    List<Text> timeTextList = new List<Text>();

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

        TimerInitialize();
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
        //時間の計測
        if (!timeCounter.isTimeOver) //&& !TutorialScript.IsTutorial)
        {
            UpdateTimeCounter();
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
    /// 時間関係の初期化
    /// </summary>
    void TimerInitialize()
    {

        timeCounter.deltaSecond = 0f;
        timeCounter.isTimeOver = false;


        var timeTextArray = GameObject.FindGameObjectsWithTag(timeCounter.timeTextObjectTag);
        if (timeTextArray.Length == 0)
        {
            Debugger.Log("timeTextというタグのゲームオブジェクトが存在しません。");
        }


        foreach (var timeText in timeTextArray)
        {
            var textCompornent = timeText.GetComponent<Text>();
            if (textCompornent == null)
            {
                Debugger.Log("Textのコンポーネントが存在しません。");
            }
            else
            {
                timeTextList.Add(textCompornent);
            }
        }
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


    /// <summary>
    /// 時間計測部の更新
    /// </summary>
    void UpdateTimeCounter()
    {
        timeCounter.deltaSecond += Time.deltaTime;

        if (timeCounter.deltaSecond >= timeCounter.maxSecond)
        {
            timeCounter.isTimeOver = true;
        }

        UpdateTimeText();
    }

    void UpdateTimeText()
    {
        foreach (var timeText in timeTextList)
        {
            timeText.text = (timeCounter.maxSecond - timeCounter.deltaSecond).ToString();
        }
    }


    /// <summary>
    /// 書かないといけない関数
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }


    

}
