//-------------------------------------------------------------
//  シーケンスのマネージャークラス
//  各オブジェクトの処理をここに記述する。
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

/// <summary>
/// シーンID
/// </summary>
public enum SceneID
{
    CONNECTION,         // コネクション
    TITLE,              // タイトル
    CHARACTER_SELECT,   // キャラクター選択
    GAME,               // ゲーム
    RESULT,             // リザルト
    MAX,
}


public class SequenceManager : Singleton<SequenceManager>
{
    /// <summary>
    /// シーン情報
    /// </summary>
    [System.Serializable]
    public struct SceneInfoData
    {
        public SceneID scene;
        public SequenceBehaviour behaviour;
    }
    
    /// <summary>
    /// シーンリスト
    /// </summary>
    [SerializeField]
    SceneInfoData[] sceneList = new SceneInfoData[(int)SceneID.MAX];

    /// <summary>
    /// 今のシーン情報
    /// </summary>
    [SerializeField]
    SceneID nowScene = SceneID.CONNECTION;

    [SerializeField]
    bool isBuildWatch = false;

    [SerializeField]
    GameObject singleCamera = null;

    [SerializeField]
    GameObject arCamera = null;

    [SerializeField]
    GameObject imageTarget = null;


    public bool IsBuildWatch { get { return isBuildWatch; } }

    public bool IsNowCharacterSelectScene { get { return nowScene == SceneID.CHARACTER_SELECT; } }
    public bool IsNowGameScene { get { return nowScene == SceneID.GAME; } }
    public bool IsNowConnnectionScene { get { return nowScene == SceneID.CONNECTION; } }
    public bool IsNowResultScene { get { return nowScene == SceneID.RESULT; } }
    public bool IsNowTitleScene { get { return nowScene == SceneID.TITLE; } }

    public GameObject SingleCamera { get { return singleCamera; } }
    public GameObject ARCamera { get { return arCamera; } }

    public override void Awake() 
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;

        base.Awake();

#if UNITY_EDITOR
        isBuildWatch = true;
        
        // ImageTargetObjectが消えないようにしている。
        var behaviours = imageTarget.GetComponents<Behaviour>();
        foreach (var behaviour in behaviours)
        {
            Destroy(behaviour);
        }

        imageTarget.transform.SetParent(sceneList[(int)SceneID.GAME].behaviour.transform);
#endif

        for (int i = 0; i < sceneList.Length; i++)
        {
            sceneList[i].behaviour.Reset();

            if (sceneList[i].scene == nowScene)
            {
                sceneList[i].behaviour.gameObject.SetActive(true);
            }
        }

        if (isBuildWatch)
        {
            singleCamera.SetActive(true);
            arCamera.SetActive(false);
        }
        else
        {
            singleCamera.SetActive(false);
            arCamera.SetActive(true);
        }
    }

    public override void Start() 
    {
        base.Start();

        //緒方追記、スリープに入らないように？（テスト成功）
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    /// <summary>
    /// 次のシーンに行かせる処理です。
    /// 切り替えたいシーンを引数で設定してください。
    /// </summary>
    /// <param name="nextScene"></param>
    public void ChangeScene(SceneID nextScene)
    {
        // 例外処理 ---------------------------------------------//

        if (nextScene == SceneID.MAX)
        {
            Debugger.LogError("そんなシーンはありません。");
        }

        if (nextScene == nowScene)
        {
            Debugger.LogWarning("同じシーンが設定されています。");
        }

        // -------------------------------------------------------//

        // 次のシーンを表示
        sceneList[(int)nextScene].behaviour.gameObject.SetActive(true);

        // 現在のシーンを非表示
        sceneList[(int)nowScene].behaviour.gameObject.SetActive(false);

        // 現在のシーンの終了処理
        sceneList[(int)nowScene].behaviour.Finish();

        nowScene = nextScene;

        Debugger.Log(">> ChangeScene");
        Debugger.Log(nowScene.ToString());
    }

    public override void Update()
    {
        base.Update();

        if (Input.touchCount == 3 || Input.GetKeyDown(KeyCode.R))
        {
            Debugger.Reset();
            PhotonNetwork.LeaveRoom();
            Application.LoadLevel(0);
        }

        if (Input.touchCount >= 4 || Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }


    /// <summary>
    /// Sequenceを取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public SequenceBehaviour GetSequence(SceneID id)
    {
        for (int i = 0; i < sceneList.Length; i++)
        {
            if (sceneList[i].scene == id)
            {
                return sceneList[i].behaviour;
            }
        }

        return null;
    }

}
