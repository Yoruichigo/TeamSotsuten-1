/*
 *  職業のデータベースのスクリプト
 * 
 *  決め事
 * 
 *  命名規則：   Pascal形式　例) AttackCount; Camel形式　attackCount
 *      名前空間 Pascal形式　クラス、構造体　Pascal形式　プロパティ　Pascal形式
 *      メンバ変数(フィールド)　Camel形式　メソッド　Pascal形式　パラメータ　Camel形式
 *      enum型   Pascal形式　
 *      enum（中身）、定数 「大文字」と「単語の区切りにアンダーバー」の組み合わせで命名する。
 *      例) ATTACK_COUNT_MAX
 *      
 *  メソット    1メソッド10行以内　最大2インデント　名前をわかりやすく
 *  Property    getのみ行う　setは、プライベート
 * 
 *  SendMessageを使わない　Editorから読み込むだけなら[Serialize Failed]を使用する
 * 
 *  状態管理をしっかり行う　ジェネリック思考で考える
 *  ほぼ全てに、コメントを書く
 *  多重ループを使用する場合、分かりやすい単語にする
 *
 * 
 * Code by shinnnosuke hiratsuka
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class JobDB : MonoBehaviour {


    //public enum JobType
    //{
    //    NEAT = -1,       //  エラーコード
    //    FENCER,     //  剣士
    //    MAGICIAN,   //  魔法使い
    //    MAX,
    //}

    ///// <summary>
    ///// 職業データの登録
    ///// </summary>
    ///// データ内に追加したい要素(職業の説明など)がありましたら追加してください。
    ///// インスペクター上でデータを適応させてください。
    //[System.Serializable]
    //public struct JobData
    //{
    //    /// <summary>
    //    /// 職業データ
    //    /// </summary>
    //    /// <param name="job">職業</param>
    //    /// 職業のデータが増えるのであれば以下に追加してください
    //    /// 例)職業の説明など
    //    public JobData(GameObject job, JobType jobType)
    //    {
    //        this.job = job;
    //        this.jobType = jobType;

    //    }

    //    public GameObject job;
    //    public JobType jobType;
    //}

    ///// <summary>
    ///// 職業データ
    ///// </summary>
    //[SerializeField]
    //private List<JobData> JobDataBase = new List<JobData>();

    //[SerializeField]
    //Text jobSelectType = null;

    ///// <summary>
    ///// 職業を設定する。
    ///// @changed m_yamada
    ///// </summary>
    ///// <param name="nextIndex"></param>
    //public void SetSelectJobType(ref int selectedNum)
    //{
    //    if (selectedNum < 0)
    //    {
    //        selectedNum = JobTypeCount - 1;
    //    }
    //    else if (selectedNum >= JobTypeCount)
    //    {
    //        selectedNum = 0;
    //    }

    //    PlayerManager.Instance.Data.Job = GetJobDataFindArray(selectedNum).jobType;
    //    jobSelectType.text = PlayerManager.Instance.Data.Job.ToString();
    //}

    ///// <summary>
    ///// 職業を設定する。
    ///// @changed m_yamada
    ///// </summary>
    ///// <param name="nextIndex"></param>
    //public void SetSelectJobType(int selectedNum)
    //{
    //    SetSelectJobType(ref selectedNum);
    //}


    //// Use this for initialization
    //void Start()
    //{
    //    JobOffer();

    //    IsNEAT();

    //}

    ///// <summary>
    ///// 職業
    ///// </summary>
    //private void JobOffer()
    //{
    //    if(JobDataBase.Count < 1)
    //    {
    //        Debug.LogError("そもそも職業情報が適応されていません。\n職業情報を登録してください。");
    //    }
    //}

    ///// <summary>
    ///// 職業がNEATかどうか
    ///// </summary>
    ///// NEATかどうかを判断する
    ///// はじめはNEATなのでNEATならば職業を決めてください
    //private void IsNEAT()
    //{
    //    foreach (var jobData in JobDataBase)
    //    {
    //        if (jobData.jobType == JobType.NEAT)
    //        {
    //            Debug.LogError(jobData.job.name + "くんはNEATです。\nインスペクター上から職業を選択してください。");
    //        }

    //    }
    //}

    ///// <summary>
    ///// 職業データID検索編
    ///// </summary>
    ///// <param name="jobID">職業のID</param>
    ///// <returns>職業データ</returns>
    //public JobData GetJobDataFindJobId(JobType jobType)
    //{
    //    return JobDataBase.Find(job => job.jobType == jobType);
    //}
    ///// <summary>
    ///// 職業データint検索編
    ///// </summary>
    ///// <param name="num">検索したい数字</param>
    ///// <returns></returns>
    //public JobData GetJobDataFindArray(int num)
    //{
    //    return JobDataBase[num];
    //}

    ///// <summary>
    ///// 職業の数を返す
    ///// </summary>
    ///// <returns>職業の数</returns>
    //public int JobTypeCount
    //{
    //    get
    //    {
    //        /// 職業情報があるかどうか
    //        if (JobDataBase.Count >= 1)
    //        {
    //            return JobDataBase.Count;
    //        }else
    //        {
    //            return -1;  /// エラー処理
    //        }
    //    }
    //}
}
