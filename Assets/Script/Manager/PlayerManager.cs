// --------------------------------------------------
//  プレイヤーのデータを管理する
//
// code by m_yamada
// --------------------------------------------------

using UnityEngine;
using System.Collections;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerMasterData Data = null;

    /// <summary>
    /// 剣士かどうか
    /// </summary>
    public bool IsFencer { get { return Data.Job == CharacterSelectManager.JobType.FENCER; } }

    /// <summary>
    /// 魔法使いかどうか
    /// </summary>
    public bool IsMagician { get { return Data.Job == CharacterSelectManager.JobType.MAGICIAN; } }


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


        if (!SequenceManager.Instance.IsNowGameScene) return;
        Data.Position = GameManager.Instance.GetPlayerData().Position;
        Data.IsHit = GameManager.Instance.GetPlayerData().IsHit;
        GameManager.Instance.GetPlayerData().IsHit = false;

    }
}
