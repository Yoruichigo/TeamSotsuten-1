//-------------------------------------------------------------
//  キャラクター選択管理クラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelectManager : Singleton<CharacterSelectManager> {
   
    public enum JobType
    {
        NEAT = -1,       //  エラーコード
        FENCER,     //  剣士
        MAGICIAN,   //  魔法使い
        MAX,
    }

    [SerializeField]
    Text jobSelectType = null;

    [SerializeField]
    CharacterSelectSequence characterSelectSequence = null;

    JobType selectedJob = JobType.NEAT;

    [SerializeField]
    Button decisionButton = null;

    [System.Serializable]
    public struct JobButtonData
    {
        public Button button;
        public JobType type;
    }

    [SerializeField]
    JobButtonData[] jobData = new JobButtonData[2];


    public override void Awake()
    {
        base.Awake();

        for (int i = 0; i < jobData.Length; i++)
        {
            var job = jobData[i].type;
            jobData[i].button.onClick.AddListener(() => {
                Selected(job);
            });
        }

        decisionButton.onClick.AddListener(Decision);
    }

    public override void Start()
    {
        base.Start();

    }

    public override void Update()
    {
        base.Update();

    }

    /// <summary>
    /// 選択処理
    /// </summary>
    public void Selected(JobType type)
    {
        selectedJob = type;
        jobSelectType.text = selectedJob.ToString();
    }

    /// <summary>
    /// 決定処理
    /// </summary>
    public void Decision()
    {
        PlayerManager.Instance.Data.Job = selectedJob;
        characterSelectSequence.ChangeScene();
        jobSelectType.text = PlayerManager.Instance.Data.Job.ToString() ;
        SEPlayer.Instance.Play(Audio.SEID.DECISION);
    }
}
