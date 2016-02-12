//-------------------------------------------------------------
//  モーション管理クラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionManager : Singleton<MotionManager>
{
    enum State
    { 
        WAIT,
        STEP,
        CALC,
    }

    /// <summary>
    /// モーションスキルタイプ
    /// </summary>
    public enum MotionSkillType
    {
        NONE = -1,      //< 未定義

        WEAK = 0,     //< 弱
        STRENGTH = 1, //< 強
    };

    /// <summary>
    /// 今のモーションスキルタイプ
    /// </summary>
    public MotionSkillType MotionSkill { get; private set; }

    [System.Serializable]
    public class MotionWatchData
    {
        public MotionSkillType skillType = MotionSkillType.NONE;
        public float accMin = 0;
        public float accMax = 0;

        [HideInInspector]
        public int clearIndex = 0;
    }

    [SerializeField]
    float calcTime = 0.5f;

    [SerializeField]
    float intervalTime = 1.0f;

    [SerializeField]
    MotionWatchData[] motionData = new MotionWatchData[2];

    [SerializeField]
    AttackSkillCreator attackSkill = null;

    State state = State.CALC;
    float countTime = 0;

    /// <summary>
    /// 加速度の一時保存データ
    /// </summary>
    Vector3 saveAcc = Vector3.zero;

    /// <summary>
    /// 常に取得する加速度データ
    /// </summary>
    Vector3 updateAcc = Vector3.zero;

    public override void Awake()
    {
        base.Awake();

        MotionSkill = MotionSkillType.NONE;

    }

    public override void Start()
    {
        base.Start();

#if !UNITY_EDITOR
        if (!ConnectionManager.IsSmartPhone)
        {
            enabled = false;
        }
#endif

    }


    /// <summary>
    /// モーションの種類を計算し、設定する。
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    bool CalcWithSetMotion(MotionWatchData data)
    {
        bool isComplated = false;

        countTime += Time.deltaTime;
        if (countTime >= calcTime)
        {
            float x = Mathf.Abs(updateAcc.x - saveAcc.x);
            float y = Mathf.Abs(updateAcc.y - saveAcc.y);
            float z = Mathf.Abs(updateAcc.z - saveAcc.z);

            bool isRangeX = x >= data.accMin && x <= data.accMax;
            bool isRangeY = y >= data.accMin && y <= data.accMax;
            bool isRangeZ = z >= data.accMin && z <= data.accMax;

            if (isRangeX || isRangeY || isRangeZ)
            {
                OnComplated(data.skillType);
                isComplated = true;
            }

            countTime = 0;
        }

        return isComplated;
    }

    /// <summary>
    /// モーションが成功した時に呼ばれる。
    /// </summary>
    void OnComplated(MotionSkillType type)
    {
        if (type == MotionSkillType.NONE) return;

        MotionSkill = type;

        if (SequenceManager.Instance.IsNowGameScene)
        {
            attackSkill.OnMotionComplated();
        }

        Debugger.Log("【モーション成功】");
        Debugger.Log(MotionSkill.ToString());
    }

    public override void Update()
    {
        base.Update();

        updateAcc.x = Mathf.Abs(WatchManager.Instance.Acc.x);
        updateAcc.y = Mathf.Abs(WatchManager.Instance.Acc.y);
        updateAcc.z = Mathf.Abs(WatchManager.Instance.Acc.z);

        switch (state)
        {
            case State.CALC:
                for (int i = 0; i < motionData.Length; i++)
                {
                    if (CalcWithSetMotion(motionData[i]))
                    {
                        state = State.STEP;
                        countTime = 0;
                        break;
                    }
                }
                break;

            case State.STEP:
                MotionSkill = MotionSkillType.NONE;
                state = State.WAIT;
                break;

            case State.WAIT:
                countTime += Time.deltaTime;
                if (countTime >= intervalTime)
                {
                    countTime = 0;
                    state = State.CALC;
                    saveAcc = updateAcc;
                }
                break;
        }



#if UNITY_EDITOR    // テストコード
        if (state != State.CALC) return;

        if (Input.GetKeyDown(KeyCode.V))
        {
            MotionSkill = MotionSkillType.WEAK;
            OnComplated(MotionSkill);
            state = State.WAIT;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            MotionSkill = MotionSkillType.STRENGTH;
            OnComplated(MotionSkill);
            state = State.WAIT;
        }
#endif
    }
}
