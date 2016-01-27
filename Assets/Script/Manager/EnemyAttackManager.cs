using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttackManager : Singleton<EnemyAttackManager> {

    [SerializeField]
    int createNum = 5;


    class AttackData
    {
        public List<EnemyAttackEffectMover> attackObjList = new List<EnemyAttackEffectMover>();
        public float speed = 5.0f;
    }

    AttackData data = new AttackData();

    int createIndex = 0;

    public override void Awake()
    {
        base.Awake();
    }

    public void Create(GameObject attackEffect,float speed)
    {
        for (int i = 0; i < data.attackObjList.Count; i++)
        {
            Destroy(data.attackObjList[i].gameObject);
        }

        data.attackObjList.Clear();

        for (int i = 0; i < createNum; i++)
        {
            var obj = Instantiate(attackEffect);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            obj.transform.position = new Vector3(0, 0, -100000);    // どこか果てしなく飛ばす.
            data.attackObjList.Add(obj.GetComponent<EnemyAttackEffectMover>());
        }

        data.speed = speed;
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
        data.attackObjList[createIndex].gameObject.SetActive(true);
        data.attackObjList[createIndex].transform.position = position;
        data.attackObjList[createIndex].Init(data.speed);
        createIndex++;

        if (data.attackObjList.Count <= createIndex)
        {
            createIndex = 0;
        }
    }

    /// <summary>
    /// すべて隠す
    /// </summary>
    public void AttackAllHide()
    {
        for (int i = 0; i < data.attackObjList.Count; i++)
        {
            data.attackObjList[i].gameObject.SetActive(false);
        }

        createIndex = 0;
    }
}
