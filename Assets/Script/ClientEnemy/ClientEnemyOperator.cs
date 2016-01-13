//-------------------------------------------------------------
// データのやり取りを管理するクラス
//
// code by m_yamada featuring Gai
//-------------------------------------------------------------
using UnityEngine;
using System.Collections;

// サーバーとの通信を行う
public class ClientEnemyOperator : MonoBehaviour 
{

    [SerializeField]
    float attackTiming = 2.0f;

    [SerializeField]
    float flashTime = 0.5f;    // 点滅する時間


    [SerializeField]
    Color defaultColor = new Color(1f, 1f, 1f, 1f);

    [SerializeField]
    Color flashColor = new Color(1f, 0f, 0f, 1f);

    SpriteRenderer spriteRenderer = null;

    bool isLive = false;
    float countTime = 0;
    float attackTime = 0;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    /// <summary>
    /// 攻撃のタイミングかどうか
    /// </summary>
    /// <returns></returns>
    bool IsAttackTiming()
    {
        attackTime -= Time.deltaTime;
        if (attackTime <= 0)
        {
            attackTime = attackTiming;
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        // 確認用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hit();
        }
#endif

        switch (EnemyManager.Instance.GetActiveEnemyData().State)
        { 
            case EnemyData.EnamyState.ACTIVE:

                if (!Vuforia.VuforiaBehaviour.IsMarkerLookAt) return;

                // プレイヤーの方向に向く
                transform.LookAt(new Vector3(GameManager.Instance.GetPlayerData().Position.x, transform.position.y, GameManager.Instance.GetPlayerData().Position.z));
                
                if (IsAttackTiming())
                {
                    EnemyAttackManager.Instance.CreateAttack(transform.position - new Vector3(0,0,0));
                }

                break;

            case EnemyData.EnamyState.HIT:
                Hit();

                countTime -= Time.deltaTime;
                if (countTime <= 0)
                {
                    HitEffectCompleted();
                }

                break;

            case EnemyData.EnamyState.SPAWN:
                Spawn();
                break;

            case EnemyData.EnamyState.STAY:
                break;

            case EnemyData.EnamyState.DEAD:
                Dead();
                break;
        }

    }

    /// <summary>
    /// 発生
    /// </summary>
    void Spawn()
    {
        if (isLive) return;

        isLive = true;

        ChangeActive();

        Debugger.Log(">> Spawn()");

        var hash = new Hashtable();
        {
            hash.Add("scale", new Vector3(0.1f, 0.1f, 0.1f)); // 設定するサイズ
            hash.Add("time", 1f);                       // 1秒で行う
            hash.Add("easetype", iTween.EaseType.easeOutQuad);        // イージングタイプを設定
            hash.Add("oncomplete", "SpawnCompleted");     // 最後にメソッドを呼ぶ
        }

        iTween.ScaleTo (this.gameObject, hash);

        attackTime = attackTiming;
    }

    void SpawnCompleted()
    {
        Debugger.Log(">> SpawnCompleted()");

        EnemyManager.Instance.GetActiveEnemyData().StateChange(EnemyData.EnamyState.ACTIVE);
    }

    /// <summary>
    /// 死亡
    /// </summary>
    void Dead()
    {
        if (!isLive) return;

        isLive = false;

        var hash = new Hashtable();
        {
            hash.Add("scale", new Vector3(0f, 0f, 0f)); // 設定するサイズ
            hash.Add("time", 1f);                       // 1秒で行う
            hash.Add("easetype", iTween.EaseType.easeOutQuad);        // イージングタイプを設定
            hash.Add("oncomplete", "ChangeActive");     // 最後にメソッドを呼ぶ
        }

        iTween.ScaleTo(this.gameObject, hash);
    }

    /// <summary>
    /// 衝突時の処理
    /// </summary>
    void Hit()
    {
        if (!EnemyManager.Instance.GetActiveEnemyData().IsHit()) return;

        // 光らせる数
        const float flashNum = 3;

        countTime = flashTime * flashNum;

        var hash = new Hashtable();
        {
            hash.Add("from", defaultColor); // 設定するサイズ
            hash.Add("to", flashColor); // 設定するサイズ
            hash.Add("time", flashTime);                       // 1秒で行う
            hash.Add("easetype", iTween.EaseType.easeOutQuad);        // イージングタイプを設定
            hash.Add("looptype", iTween.LoopType.pingPong);        // イージングタイプを設定
            hash.Add("onupdate", "ColorUpdateHandler");        // イージングタイプを設定
        }

        iTween.ValueTo(gameObject, hash);

        // HitEffect再生 座標の-30は、敵の手前に出す数値
        HitEffectManager.Instance.EffectPlay(
            EnemyManager.Instance.GetActiveEnemyData().HitSkillType(), 
            transform.position - new Vector3(0,0,30));

        EnemyManager.Instance.GetActiveEnemyData().HitRelease();
    }

    void ColorUpdateHandler(Color color)
    {
        spriteRenderer.color = color;
    }
    
    void HitEffectCompleted()
    {
        iTween.Stop(gameObject);

        Debugger.Log(">> HitEffectCompleted()");

        EnemyManager.Instance.GetActiveEnemyData().StateChange(EnemyData.EnamyState.ACTIVE);
        spriteRenderer.color = defaultColor;
    }

    /// <summary>
    /// アクティブ状態を変更する
    /// </summary>
    void ChangeActive()
    {
        GameManager.Instance.SendEnemyIsActive(EnemyManager.Instance.GetActiveEnemyData().Id, isLive);
    }
}
