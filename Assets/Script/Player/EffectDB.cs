///-------------------------------------------------------------------------
///
/// code by miyake yuji
///
/// エフェクトのデータベース
/// 
///-------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EffectDB : MonoBehaviour {

    
    /// <summary>
    /// エフェクトのデータベース用構造体
    /// </summary>
    public struct EffectData
    {
        /// <summary>
        /// 攻撃タイプ
        /// </summary>
        public MotionManager.MotionSkillType skillType;

        /// <summary>
        /// 攻撃の速度
        /// </summary>
        public float speed;

        /// <summary>
        /// ダメージ量
        /// </summary>
        public int damage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_type">攻撃タイプ</param>
        /// <param name="_speed">速度</param>
        /// <param name="_scale">サイズ</param>
        /// <param name="_damage">ダメージ</param>
        public EffectData(
            MotionManager.MotionSkillType _type , 
            float _speed,
            int _damage
            )
        {
            skillType = _type;
            speed = _speed;
            damage = _damage;
        }
    }

    /// <summary>
    /// 各エフェクトの細かいデータ一覧
    /// 順番は攻撃タイプの列挙型に準拠
    /// </summary>
    private float[] speedArray = new float[] 
    {
        1000.0f,1500.0f
    };

    /// <summary>
    /// ダメージ配列
    /// </summary>
    private int[] damageArray = new int[] { 5, 10 };

    /// <summary>
    /// ゲーム中に参照するエフェクトデータのリスト
    /// </summary>
    public List<EffectData> dataList = new List<EffectData>() { };

	// Use this for initialization
	void Start ()
    {

        // NONEを引いた値をカウントにする。
        var valueCount = Enum.GetValues(typeof(MotionManager.MotionSkillType)).Length - 1;

        ///<summary>
        /// データをリストへ入れる 
        ///</summary>
        for (int i = 0;i < valueCount;i++)
        {
            var motionType = (MotionManager.MotionSkillType)i;
            EffectData data = new EffectData(
                motionType,
                speedArray[i],
                damageArray[i]);

            dataList.Add(data);
        }


	}
}
