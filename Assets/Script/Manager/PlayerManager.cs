﻿// --------------------------------------------------
//  プレイヤーのデータを管理する
//
// code by m_yamada
// --------------------------------------------------

using UnityEngine;
using System.Collections;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerMasterData Data = null;

    [SerializeField]
    CameraShaker cameraShaker = null;

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

#if !UNITY_EDITOR
        if (!ConnectionManager.IsSmartPhone) return;
#endif

        if (!SequenceManager.Instance.IsNowGameScene) return;

        Data.Position = GameManager.Instance.GetPlayerData().Position;
        Data.IsHit = GameManager.Instance.GetPlayerData().IsHit;

        if (Data.IsHit)
        {
            cameraShaker.Shake();
#if UNITY_ANDROID
            Handheld.Vibrate(); // バイブ
#endif
            SEPlayer.Instance.Play(Audio.SEID.DAMAGE);
            GameManager.Instance.GetPlayerData().IsHit = false;
        }

    }
}
