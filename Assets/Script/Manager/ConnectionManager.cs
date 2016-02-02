//-------------------------------------------------------------
//  コネクション管理クラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConnectionManager : Singleton<ConnectionManager>
{
    enum TerminalType
    {
        Null = -1,
        Phone,
        Watch,
    }

    [SerializeField]
    SceneID nextSceneID = SceneID.CHARACTER_SELECT; 

    [SerializeField]
    string roomName = "Room";

    /// <summary>
    /// 接続最大人数
    /// </summary>
    [SerializeField]
    byte roomMaxPlayerNum = 5;

    PhotonView view = null;

    List<Button> buttonList = new List<Button>();
    List<Text> textList = new List<Text>();

    /// <summary>
    /// 接続ID
    /// </summary>
    static public int ID { get; private set; }

    static TerminalType type = TerminalType.Null;

    // 接続数
    static public int SmartPhoneConnectionNum = 0;
    static public int WatchConnectionNum = 0;

    // 自分がどの端末で接続しているか
    static public bool IsWacth { get { return type == TerminalType.Watch; } }
    static public bool IsSmartPhone { get { return type == TerminalType.Phone; } }

    /// <summary>
    /// スマホのプレイヤー情報
    /// </summary>
    static PhotonPlayer smartPhonePlayer = null;

    /// <summary>
    /// ウォッチのプレイヤー情報
    /// </summary>
    static PhotonPlayer watchPlayer = null;

    /// <summary>
    /// スマホのプレイヤーを取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    static public PhotonPlayer GetSmartPhonePlayer()
    {
        return smartPhonePlayer;
    }

    /// <summary>
    /// ウォッチのプレイヤーを取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    static public PhotonPlayer GetWatchPlayer()
    {
        return watchPlayer;
    }

    public override void Awake()
    {
        base.Awake();

        //マスターサーバーへ接続
        PhotonNetwork.offlineMode = false;
        PhotonNetwork.ConnectUsingSettings("v0.1");

        view = GetComponent(typeof(PhotonView)) as PhotonView;

    }


    /// <summary>
    /// 名前でテキストを取得
    /// </summary>
    /// <param name="findName"></param>
    /// <returns></returns>
    Text[] GetTextsByName(string findName)
    {
        return textList.FindAll(i => i.name == findName).ToArray();
    }

    /// <summary>
    /// 名前でボタンを取得
    /// </summary>
    /// <param name="findName"></param>
    /// <returns></returns>
    Button[] GetButtonsByName(string findName)
    {
        return buttonList.FindAll(i => i.name == findName).ToArray();
    }


    public override void Start()
    {
        base.Start();

        MotionManager.Instance.gameObject.SetActive(false);
        HitEffectManager.Instance.gameObject.SetActive(false);

        var canvasRoot = transform.parent.GetComponentsInChildren<Canvas>();

        foreach (var canvas in canvasRoot)
        {
            var buttons = canvas.GetComponentsInChildren<Button>();

            foreach (var button in buttons)
            {
                buttonList.Add(button);
            }

            var texts = canvas.GetComponentsInChildren<Text>();

            foreach (var text in texts)
            {
                textList.Add(text);
            }
        }

        foreach (var button in buttonList)
        {
            var temp = button;
            button.onClick.AddListener(() => {
                OnButtonListener(temp);
            });
        }

        var getItems = GetButtonsByName("EegressButton");
        for(int i = 0 ;i<getItems.Length;i++)
        {
            getItems[i].gameObject.SetActive(false);
        }
    }

    public void OnButtonListener(Button clickButton)
    {
        SEPlayer.Instance.Play(Audio.SEID.DECISION);

        if (clickButton.name == "EegressButton")
        {
            OnEegress();
        }
        if (clickButton.name == "SmartPhoneButton")
        {
            OnPhoneConnection();
        }
        if (clickButton.name == "WatchButton")
        {
            OnWatchConnection();
        }

    }

    public override void Update()
    {
        base.Update();

        if(PhotonNetwork.isMasterClient)
        {
            // 2人なら、もう一つの要素も2にする。
            if (WatchConnectionNum >= 1)
            {
                // すべての接続が完了したら処理する。
                view.RPC("AllCompletion", PhotonTargets.All);

                if (Input.GetMouseButtonDown(0))    // テストコード
                {
                    // ホストが持っている端末情報を子機へ同期させる。
                    // すべての端末がホストの各端末情報をもらう。
                    var data = new object[] { smartPhonePlayer, watchPlayer };
                    view.RPC("SyncTerminalInfo", PhotonTargets.All, data);
                    view.RPC("SyncChangeScene", PhotonTargets.All);
                }
            }
        }
    }

    /// <summary>
    /// オール接続完了
    /// </summary>
    /// <param name="watch"></param>
    /// <param name="info"></param>
    [PunRPC]
    void AllCompletion(PhotonMessageInfo info)
    {
        var getItems = GetTextsByName("ConnectionStateText");
        for (int i = 0; i < getItems.Length; i++)
        {
            getItems[i].text = "接続完了";
        }
    }

    /// <summary>
    /// すべてのConnectionボタンをDisableにする。
    /// </summary>
    void AllConnectionButtonDisable()
    {
        var getItems = GetButtonsByName("SmartPhoneButton");
        for (int i = 0; i < getItems.Length; i++)
        {
            getItems[i].enabled = false;
        }

        getItems = GetButtonsByName("WatchButton");
        for (int i = 0; i < getItems.Length; i++)
        {
            getItems[i].enabled = false;
        }
    }

    /// <summary>
    /// スマホでのコネクションアクション
    /// </summary>
    public void OnPhoneConnection()
    {
        Debugger.Log(">> OnPhoneConnection()");

        StartCoroutine("OnConnection");
        type = TerminalType.Phone;

        AllConnectionButtonDisable();
    }

    /// <summary>
    /// ウォッチのコネクションアクション
    /// </summary>
    public void OnWatchConnection()
    {
        Debugger.Log(">> OnWatchConnection()");

        StartCoroutine("OnConnection");
        type = TerminalType.Watch;

        var getItems = GetButtonsByName("WatchButton");
        for (int i = 0; i < getItems.Length; i++)
        {
            getItems[i].enabled = false;
        }

        AllConnectionButtonDisable();

    }


    /// <summary>
    /// コネクションする。
    /// </summary>
    IEnumerator OnConnection()
    {
        yield return new WaitForSeconds(1.0f);

        // 部屋設定
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.isOpen = true;      // 部屋を開くか
        roomOptions.isVisible = true;   // 一覧に表示するか
        roomOptions.maxPlayers = roomMaxPlayerNum;

        // 部屋に参加、存在しない時作成して参加
        bool isJoin = PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, new TypedLobby());

        Debugger.Log("部屋に参加 : " + isJoin);
    }

    /// <summary>
    /// ルーム退室
    /// </summary>
    public void OnEegress()
    {
        view.RPC("AllEgressRoom", PhotonTargets.All);
    }

    /// <summary>
    /// スマホを設定する。
    /// 配列に入れていく。
    /// </summary>
    /// <param name="player"></param>
    /// <param name="info"></param>
    [PunRPC]
    public void SetSmartPhoneID(PhotonPlayer player, PhotonMessageInfo info)
    {
        smartPhonePlayer = player;
        
        var debugIndex = SmartPhoneConnectionNum + 1;
        Debugger.Log("スマホ_" + debugIndex + " : 接続");
        
        SmartPhoneConnectionNum++;
    }

    /// <summary>
    /// ウォッチを設定する。
    /// 配列に入れていく。
    /// </summary>
    /// <param name="player"></param>
    /// <param name="info"></param>
    [PunRPC]
    public void SetWatchID(PhotonPlayer player, PhotonMessageInfo info)
    {
        watchPlayer = player;

        var debugIndex = WatchConnectionNum + 1;
        Debugger.Log("ウォッチ_" + debugIndex + " : 接続");

        WatchConnectionNum++;
    }


    /// <summary>
    /// ルームの接続状態をアクティブにする。
    /// </summary>
    /// <param name="info"></param>
    [PunRPC]
    public void ActiveRoomState(PhotonMessageInfo info)
    {
        var getItems = GetTextsByName("ConnectionStateText");
        for (int i = 0; i < getItems.Length; i++)
        {
            getItems[i].enabled = true;
            getItems[i].text = "接続中";
        }
    }

    /// <summary>
    /// 端末情報を取得
    /// </summary>
    /// <param name="id"></param>
    /// <param name="phone"></param>
    /// <param name="watch"></param>
    [PunRPC]
    void SyncTerminalInfo(PhotonPlayer phone, PhotonPlayer watch, PhotonMessageInfo info)
    {
        Debugger.Log(">> 端末情報を同期します。");

        watchPlayer = watch;
        smartPhonePlayer = phone;
    }

    /// <summary>
    /// シーンを切り替える。　同期
    /// </summary>
    /// <param name="info"></param>
    [PunRPC]
    public void SyncChangeScene(PhotonMessageInfo info)
    {
        SequenceManager.Instance.ChangeScene(nextSceneID);
        MotionManager.Instance.gameObject.SetActive(true);
        HitEffectManager.Instance.gameObject.SetActive(true);

        if (IsWacth)
        {
            WatchManager.Instance.CanvasActive();
        }
    }
    
    /// <summary>
    /// マスターサーバーのロビー入室時
    /// </summary>
    void OnJoinedLobby()
    {
        Debugger.Log(">> ロビー入室");
    }

    /// <summary>
    /// ルーム参加時
    /// </summary>
    void OnJoinedRoom()
    {
        Debugger.Log(">> ルーム参加");

        var index = PhotonNetwork.playerList.Length - 1;

        if (!PhotonNetwork.isMasterClient)
        {
            ID = SmartPhoneConnectionNum;
            Debugger.Log("ID : " + ID);
        }

        var getItem = GetTextsByName("ConnectionStateText");
        for (int i = 0; i < getItem.Length; i++)
        {
            getItem[i].enabled = true;
            getItem[i].text = "接続待ち";
        }

        switch (type)
        {
            case TerminalType.Phone:
                view.RPC("SetSmartPhoneID", PhotonTargets.All, new object[] { PhotonNetwork.playerList[index] });
                break;

            case TerminalType.Watch:

                view.RPC("SetWatchID", PhotonTargets.All, new object[] { PhotonNetwork.playerList[index] });
                view.RPC("ActiveRoomState", PhotonTargets.All);
                break;

            default:
                Debugger.LogError("端末の種類を設定できていません。");

                break;
        }

        var getItems = GetButtonsByName("EegressButton");
        for (int i = 0; i < getItems.Length; i++)
        {
            getItems[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 全員を退出
    /// </summary>
    /// <param name="info"></param>
    [PunRPC]
    public void AllEgressRoom(PhotonMessageInfo info)
    {
        // ルーム退室
        PhotonNetwork.LeaveRoom();

        Application.LoadLevel(0);
    }


    /// <summary>
    /// 他のユーザーのルーム退室時
    /// </summary>
    /// <param name="otherPlayer">Other player.</param>
    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debugger.Log("他の人が退出しました。");

    }

    /// <summary>
    /// 他ユーザーがルームに接続した時
    /// </summary>
    /// <param name="newPlayer">New player.</param>
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debugger.Log("他の人が接続しました");
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
