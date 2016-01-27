//-------------------------------------------------------------
//  攻撃スキル発生クラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class AttackSkillCreator : MonoBehaviour
{

    /// <summary>
    /// スキルを生成
    /// 生成できるようになると、この関数が呼ばれます。
    /// ここに発生するエフェクト等を記述してください。
    /// 
    /// ・switch文での分岐をif文での分岐に変更
    /// ・effectMoverにて判断 
    /// ・呼び出すオブジェクトがアクティブかを確認する
    /// update by miyake
    /// </summary>
    public void OnMotionComplated()
    {
        if (TutorialScript.IsTutorial) return;

        if (MotionManager.Instance.MotionSkill != MotionManager.MotionSkillType.NONE)
        {
            PlayerAttackEffectManager.Instance.CheckType(MotionManager.Instance.MotionSkill);
        }
    }
}
