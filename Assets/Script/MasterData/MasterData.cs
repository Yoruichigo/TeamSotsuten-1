//-------------------------------------------------------------
//  マスターデータ クラス
// 
//  code by m_yamada
//  and ogata
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;


/// <summary>
/// プレイヤーマスターデータ(一人一人)
/// </summary>
[System.Serializable] 
public class PlayerMasterData 
{
    public Vector3 Position; //座標

    public JobDB.JobType Job;   //職業

    public int HelthPoint;

    public bool IsHit;  //< ヒットフラグ
    
}


/// <summary>
/// エネミーマスターデータ(１体１体)
/// </summary>
public class EnemyMasterData
{
    public int HP;      //体力

    public Vector3 Position;    // 座標

    public Vector3 Rotation;    //方向

    public bool IsActive; // 有効化フラグ

    public EnemyData.EnamyState State;  // 状態

    public bool IsHit;  // 攻撃に当たった場合、有効化します。

    public MotionManager.MotionSkillType HitAttackType; // どの攻撃が当たったか

}




