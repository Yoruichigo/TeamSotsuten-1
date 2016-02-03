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
    const string TWEEN_START_SCALE = "TitleStartScale";
    const string TWEEN_START_COLOR = "TitleStartColor";

    uTweenBase[] scaleTweenList = null;
    bool isStartAnim = false;


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
#if !UNITY_EDITOR
        if (!ConnectionManager.IsSmartPhone) return;
#endif

        if (!isStartAnim)
        {
            if (MotionManager.Instance.MotionSkill == MotionManager.MotionSkillType.STRENGTH)
            {
                isStartAnim = true;
                
                scaleTweenList = uTween.GetPlayList(TWEEN_START_SCALE);
                uTween.Plays(TWEEN_START_SCALE);
                uTween.Plays(TWEEN_START_COLOR);

                SEPlayer.Instance.Play(Audio.SEID.DECISION);
            }
        }
        else
        {
            if (!scaleTweenList[0].IsPlaying)
            {
                isStartAnim = false;

#if !UNITY_EDITOR
                view.RPC("ChangeScene", PhotonTargets.All);
#else
                SequenceManager.Instance.ChangeScene(SceneID.GAME);
#endif
            }
        }
	}


    [PunRPC]
    void ChangeScene(PhotonMessageInfo info)
    {
        SequenceManager.Instance.ChangeScene(SceneID.GAME);
    }
}
