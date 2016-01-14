//-------------------------------------------------------------
//  キャラクター選択管理クラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class CharacterSelectManager : Singleton<CharacterSelectManager> {

    /// <summary>
    /// 選択中の職業
    /// @changed m_yamada
    /// </summary>
    public JobDB.JobType SelectedJobType = JobDB.JobType.FENCER;

    /// <summary>
    /// 剣士かどうか
    /// </summary>
    public bool IsFencer { get { return SelectedJobType == JobDB.JobType.FENCER; } }

    /// <summary>
    /// 魔法使いかどうか
    /// </summary>
    public bool IsMagician { get { return SelectedJobType == JobDB.JobType.MAGICIAN; } }


    public override void Awake()
    {
        base.Awake();

    }

    public override void Start()
    {
        base.Start();

    }

    public override void Update()
    {
        base.Update();

    }
}
