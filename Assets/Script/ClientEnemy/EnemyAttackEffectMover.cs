using UnityEngine;
using System.Collections;

public class EnemyAttackEffectMover : MonoBehaviour {

    // ヒット範囲
    const float HIT_RANGE = 300;

    Rigidbody cashRigidBody = null;

    // Use this for initialization
    void Start()
    {
        cashRigidBody = GetComponent<Rigidbody>();
    }
	
    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(float speed)
    {
        var direction = (GameManager.Instance.GetPlayerData().Position - transform.position).normalized;
        cashRigidBody.velocity = direction * speed;
    }

	// Update is called once per frame
	void Update () 
    {
        // 距離以内なら当たったとみなす。
        if (Vector3.Distance(transform.position, SequenceManager.Instance.ARCamera.transform.position) <= HIT_RANGE)
        {
            gameObject.SetActive(false);
            GameManager.Instance.SendPlayerHit(true,2);
        }
	}


}
