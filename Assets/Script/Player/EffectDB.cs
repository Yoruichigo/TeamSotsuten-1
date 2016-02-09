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
    [Serializable
    ]public struct EffectData
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
    /// ゲーム中に参照するエフェクトデータのリスト
    /// </summary>
    public EffectData[] dataList = new EffectData[2];


}
