//-------------------------------------------------------------
//  キャラクター選択管理クラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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
        public Image icon;
        public JobType type;
    }

    [SerializeField]
    JobButtonData[] jobData = new JobButtonData[2];


    public override void Awake()
    {
        base.Awake();

        List<Sprite> spriteList = new List<Sprite>();

        for (int i = 0; i < jobData.Length; i++)
        {
            var job = jobData[i].type;
            var image = jobData[i].icon;
            jobData[i].button.onClick.AddListener(() => {
                Selected(job, ref image);
            });

            spriteList.Add(image.sprite);
        }

        WatchManager.Instance.SetJonAllSprite(spriteList.ToArray());
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
    public void Selected(JobType type,ref Image icon)
    {
        selectedJob = type;
        jobSelectType.text = selectedJob.ToString();
        WatchManager.Instance.SetJobIcon(icon.sprite);
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
