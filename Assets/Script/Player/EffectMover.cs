///-------------------------------------------------------------------------
///
/// code by miyake yuji
///
/// 攻撃エフェクト(オブジェクト)の移動スクリプト
/// 
///-------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class EffectMover : MonoBehaviour
{
    /// <summary>
    /// 動作から取得したIDを取得
    /// </summary>
    MotionManager.MotionSkillType type;

    /// <summary>
    /// ターゲットへのダメージ量
    /// </summary>
    int takeDamage;


    // ヒット範囲
    [SerializeField]
    float HIT_RANGE = 1000;

    Rigidbody cashRigidBody = null;

    // Use this for initialization
    void Awake()
    {
        cashRigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(MotionManager.MotionSkillType attackType,int damage, float speed)
    {
        //　エフェクトがどの攻撃タイプか情報を保存
        type = attackType;

        //　ダメージ量をオブジェクトに保存
        takeDamage = damage;

        transform.position = SequenceManager.Instance.ARCamera.transform.position;

        var direction = (GameManager.Instance.Enemy.transform.position - transform.position).normalized;
        cashRigidBody.velocity = direction * speed;

    }

    void Update()
    {
        if (EnemyManager.Instance.GetActiveEnemyData().State == EnemyData.EnamyState.DEAD) return;

        // 距離以内なら当たったとみなす。
        if (Vector3.Distance(transform.position, GameManager.Instance.Enemy.transform.position) <= HIT_RANGE)
        {
            OnHit();
        }
    }

    /// <summary>
    /// ヒット処理
    /// </summary>
    void OnHit()
    {
        // オブジェクトの非アクティブ化
        gameObject.SetActive(false);

        // 死んでいたら処理しないようにする。
        if (EnemyManager.Instance.GetActiveEnemyData().State == EnemyData.EnamyState.DEAD) return;

        // マネージャーへターゲットへのダメージを渡す
        GameManager.Instance.SendEnemyHit(type,takeDamage);
    }

}
