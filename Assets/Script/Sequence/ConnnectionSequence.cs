//-------------------------------------------------------------
//  コネクションシーン遷移クラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ConnnectionSequence : SequenceBehaviour
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
        BGMPlayer.Instance.Play("GameBGM", new FadeTimeData(1, 1));
    }

    void Update()
    {

    }
}
