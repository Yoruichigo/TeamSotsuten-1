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
    /// ITweenの再生中の管理
    /// </summary>
    bool itweenCheck;

    /// <summary>
    /// 動作から取得したIDを取得
    /// </summary>
    MotionManager.MotionSkillType type;

    /// <summary>
    /// ターゲットへのダメージ量
    /// </summary>
    int takeDamage;

    /// <summary>
    /// プレイヤーの挙動を受け取り攻撃タイプを判別
    /// </summary>
    /// <param name="attackType">動作から受け取った行動ID</param>
    public void OnObject(
        MotionManager.MotionSkillType attackType,
        float speed,
        GameObject target,
        GameObject player,
        Vector3 scale,
        int damage
        )
    {
        //　エフェクトオブジェクトのアクティブ化
        gameObject.SetActive(true);

        //　プレイヤーの位置からスタート
        gameObject.transform.position = player.transform.position;

        //アニメーションが動いたていたら一度止める
        iTween.Stop(gameObject);

        //　エフェクトがどの攻撃タイプか情報を保存
        type = attackType;

        //　ダメージ量をオブジェクトに保存
        takeDamage = damage;

        //　サイズ変更
        gameObject.transform.localScale = scale;

        //　ITweenの再生中か確認
        if (itweenCheck == false) Move(target.transform.position, speed);

        //　ITweenが再生中のフラグを立てる
        itweenCheck = true;

    }

    /// <summary>
    /// Move()のITweenが終了した時の座標修正とオブジェクトの非アクティブ化
    /// </summary>
    void ItweenOnComplete()
    {
        // ITweenが無効になっているフラグたて
        itweenCheck = false;

        // オブジェクトの非アクティブ化
        gameObject.SetActive(false);

        // 死んでいたら処理しないようにする。
        if (EnemyManager.Instance.GetActiveEnemyData().State == EnemyData.EnamyState.DEAD) return;

        // マネージャーへターゲットへのダメージを渡す
        GameManager.Instance.SendEnemyHit(type,takeDamage);
    }

    /// <summary>
    /// エフェクトの動きに関する挙動(ITween使用)
    /// </summary>
    void Move(Vector3 targetPosition ,float speed)
    {
        //　targetPositionに向かって等速で移動
        iTween.MoveTo(gameObject,
            iTween.Hash(
            "position", targetPosition,
            "time", speed,
            "easetype", iTween.EaseType.linear,
            "oncomplete", "ItweenOnComplete"
            ));
    }

    /// <summary>
    /// Update メソッドが最初に呼び出される前のフレームで呼び出されます
    /// </summary>
    void Start()
    {
        // ITween用のフラグ初期化
        itweenCheck = false;

        // 攻撃タイプの初期化
        type = MotionManager.MotionSkillType.NONE;
    }

}
