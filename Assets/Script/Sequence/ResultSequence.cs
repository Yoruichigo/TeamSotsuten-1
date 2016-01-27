//-------------------------------------------------------------
//  リザルトシーン遷移クラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ResultSequence : SequenceBehaviour
{

    public override void Reset()
    {
        base.Reset();
    }

    public override void Finish()
    {
        base.Finish();
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SequenceManager.Instance.ChangeScene(SceneID.TITLE);
        }
    }
}
