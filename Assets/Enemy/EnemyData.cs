///
/// 高木へ、コメント書きましょう
///
/// code by TKG and ogata 


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyData: MonoBehaviour 
{

    [SerializeField]
    int life;      //体力

    [SerializeField]
    Vector3 Position;    // 座標

    [SerializeField]
    Vector3 Rotation;    //方向

    public int Life
    {
        get { return life; }
    }

    public enum EnamyState
    {
        NONE,
        SPAWN,
        STAY,
        ACTIVE,
        HIT,
        DEAD,
        ATTACK
    }

    [SerializeField]
    EnamyState state = EnamyState.NONE;

    [SerializeField]
    GameObject attackEffect = null;

    [SerializeField]
    float attackSpeed = 1000.0f;

    [SerializeField]
    Color color = Color.white;

    [SerializeField]
    Sprite[] standingSpriteList = new Sprite[1];

    [SerializeField]
    Sprite[] attackSpriteList = new Sprite[1];

    [SerializeField]
    EnemyMasterData.ENEMY_TYPE enemyType = EnemyMasterData.ENEMY_TYPE.NULL;

    public AnimationClip AnimationAIClip = null;
    public AnimationClip AnimationSpwanClip = null;


    public EnemyMasterData.ENEMY_TYPE EnemyType { get { return enemyType; } }

    public EnamyState State{  get { return state; }}

    public void StateChange(EnamyState _statNnum) { state = _statNnum; }

    public bool IsActive() { return GameManager.Instance.GetEnemyData().IsActive; }

    public bool IsHit() { return GameManager.Instance.GetEnemyData().IsHit; }

    public MotionManager.MotionSkillType HitSkillType() { return GameManager.Instance.GetEnemyData().HitAttackType; }

    /// <summary>
    /// ヒットフラグを解除する。
    /// </summary>
    public void HitRelease()
    {
        GameManager.Instance.GetEnemyData().IsHit = false;
    }

    //GameManager用のエネミーデータ
    public void SetMyDate()
    {
        GameManager.Instance.SendEnemyHP(life);
        GameManager.Instance.SendEnemyPosition(transform.position);
        GameManager.Instance.SendEnemyRotation(transform.rotation.eulerAngles);
        GameManager.Instance.SendEnemyIsActive(true);
        GameManager.Instance.SendEnemyType(enemyType);

        EnemyManager.Instance.SetEnemySprite(ref standingSpriteList,ref attackSpriteList,ref color);

        EnemyAttackManager.Instance.Create(attackEffect, attackSpeed);
    }


    public void UpdateData()
    {
        life = GameManager.Instance.GetEnemyData().HP;
    }

}
