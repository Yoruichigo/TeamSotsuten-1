//-------------------------------------------------------------
//  ゲームシーン遷移クラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class GameSequence : SequenceBehaviour
{

    [SerializeField]
    WatchDeviceInfoRecorder watchRecorder = null;

    public override void Reset()
    {
 	     base.Reset();

         watchRecorder.StopDebugShow();
    }

    public override void Finish()
    {
        base.Finish();

        watchRecorder.StopDebugShow();

    }

	void Start () 
    {
        if (!ConnectionManager.IsSmartPhone) return;

        watchRecorder.StartDebugShow();
	}
	
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            //SequenceManager.Instance.ChangeScene(SceneID.RESULT);
        }
	}
}
