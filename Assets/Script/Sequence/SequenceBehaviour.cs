//-------------------------------------------------------------
//  シーケンスの基底クラス
//  シーン遷移クラスは、これを継承してください。
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class SequenceBehaviour : MonoBehaviour
{

    /// <summary>
    /// シーンの初期化
    /// </summary>
    public virtual void Reset()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// シーンの終了処理
    /// </summary>
    public virtual void Finish()
    {

    }
}
