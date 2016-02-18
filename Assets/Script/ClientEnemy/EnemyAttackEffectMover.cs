using UnityEngine;
using System.Collections;

public class EnemyAttackEffectMover : MonoBehaviour {

    // ヒット範囲
    const float HIT_RANGE = 300;

    int hitPower = 0;

    Rigidbody cashRigidBody = null;

    // Use this for initialization
    void Awake()
    {
        cashRigidBody = GetComponent<Rigidbody>();
    }
	
    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(float speed,int power)
    {
        hitPower = power;
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
            GameManager.Instance.SendPlayerHit(true, hitPower);
            
            if (TutorialSequence.IsTutorial)
            {
#if UNITY_EDITOR
                TutorialSequence.PlayerDodge();
#else
                TutorialSequence.ResetDodge();
#endif
            }


            return;
        }

        //チュートリアル用、プレイヤーが避けた処理
        if (TutorialSequence.IsTutorial)
        {
            if (transform.position.z < (SequenceManager.Instance.ARCamera.transform.position.z - HIT_RANGE))
            {
                gameObject.SetActive(false);
                TutorialSequence.PlayerDodge();
            }
        }


	}


}
