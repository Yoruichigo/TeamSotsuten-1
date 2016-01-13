///-------------------------------------------------------------------------
///
/// code by miyake yuji
///
/// エフェクトの作成管理スクリプト
/// 
///-------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectCreate : MonoBehaviour
{
    /// <summary>
    /// 呼び出す攻撃エフェクトを登録
    /// </summary>
    [SerializeField]
    GameObject strength = null;

    [SerializeField]
    GameObject weak = null;

    [SerializeField]
    int createNum = 5;

    List<EffectMover> weakEffectList = new List<EffectMover>();
    List<EffectMover> strengthEffectList = new List<EffectMover>();

    EffectDB db = null;

    int weakPlayIndex = 0;
    int strengthPlayIndex = 0;

    void Awake()
    {
        db = GetComponent<EffectDB>();

        for (int i = 0; i < createNum; i++)
        {
            var obj = Instantiate(strength);
            obj.transform.SetParent(transform);
            obj.gameObject.SetActive(false);
            strengthEffectList.Add(obj.GetComponent<EffectMover>());
        }

        for (int i = 0; i < createNum; i++)
        {
            var obj = Instantiate(weak);
            obj.transform.SetParent(transform);
            obj.gameObject.SetActive(false);
            weakEffectList.Add(obj.GetComponent<EffectMover>());
        }
    }

    /// <summary>
    /// どの攻撃タイプのエフェクトを生成するか確認
    /// </summary>
    /// <param name="skillType"></param>
    void CheckType(MotionManager.MotionSkillType skillType)
    {
        /// <summary>
        /// 攻撃タイプを判別
        /// </summary>
        switch (skillType)
        {
            case MotionManager.MotionSkillType.STRENGTH:
                EffectPlay(strengthEffectList, skillType, ref strengthPlayIndex);
                SEPlayer.Instance.Play("PlayerStrengthAttack");
                break;
            case MotionManager.MotionSkillType.WEAK:
                EffectPlay(weakEffectList, skillType, ref weakPlayIndex);
                SEPlayer.Instance.Play("PlayerWeakAttack");
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 攻撃のエフェクトを再生
    /// </summary>
    /// <param name="effectList"></param>
    /// <param name="skillType"></param>
    /// <param name="index"></param>
    void EffectPlay(List<EffectMover> effectList, MotionManager.MotionSkillType skillType, ref int index)
    {
        effectList[index].gameObject.SetActive(true);

        effectList[index].OnObject(
                    db.dataList[(int)skillType].skillType,
                    db.dataList[(int)skillType].speed,
                    GameManager.Instance.Enemy,
                    GameManager.Instance.Player,
                    db.dataList[(int)skillType].scale,
                    db.dataList[(int)skillType].damage
                    );

        index++;

        if (effectList.Count <= index)
        {
            index = 0;
        }
    }
}
