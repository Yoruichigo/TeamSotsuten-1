using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttackManager : Singleton<EnemyAttackManager> {

    [SerializeField]
    int createNum = 5;

    [SerializeField]
    GameObject attackEffect = null;

    [SerializeField]
    float speed = 5.0f;

    List<EnemyAttackEffectMover> attackObjList = new List<EnemyAttackEffectMover>();
    int createIndex = 0;

    public override void Awake()
    {
        base.Awake();

        // TODO 弾のレイヤをエネミーより手前にする。

        for (int i = 0; i < createNum; i++)
        {
            var obj = Instantiate(attackEffect);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            attackObjList.Add(obj.GetComponent<EnemyAttackEffectMover>());
        }
    }

    public override void Start()
    {
        base.Start();

    }

    public override void Update()
    {
        base.Update();

    }

    /// <summary>
    /// 攻撃を生成する
    /// </summary>
    /// <param name="position"></param>
    public void CreateAttack(Vector3 position)
    {
        attackObjList[createIndex].gameObject.SetActive(true);
        attackObjList[createIndex].transform.position = position;
        attackObjList[createIndex].Init(speed);
        createIndex++;

        if (attackObjList.Count <= createIndex)
        {
            createIndex = 0;
        }
    }
}
