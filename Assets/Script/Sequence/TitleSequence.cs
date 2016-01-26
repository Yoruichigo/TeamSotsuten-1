//-------------------------------------------------------------
//  タイトルシーン遷移クラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class TitleSequence : SequenceBehaviour
{
    PhotonView view = null;

    public override void Reset()
    {
        base.Reset();
    }

    public override void Finish()
    {
        base.Finish();
    }

	// Use this for initialization
	void Start () 
    {
        view = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!ConnectionManager.IsSmartPhone) return;

        if (MotionManager.Instance.MotionSkill == MotionManager.MotionSkillType.STRENGTH)
        {
            view.RPC("ChangeScene", PhotonTargets.All);
            SEPlayer.Instance.Play(Audio.SEID.DECISION);
        }
	}


    [PunRPC]
    void ChangeScene(PhotonMessageInfo info)
    {
        SequenceManager.Instance.ChangeScene(SceneID.GAME);
    }
}
