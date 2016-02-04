using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeUIInfo : MonoBehaviour
{

    // 時間表示
    [System.Serializable]
    public class TimeCounter
    {
        /// <summary>
        /// ゲーム開始時からの経過時間
        /// </summary>
        public float deltaSecond;

        /// <summary>
        /// 最大時間
        /// </summary>
        public float maxSecond;

        /// <summary>
        /// タイムオーバーをしているかどうか
        /// true...制限時間を過ぎている。 false...まだ制限時間内
        /// </summary>
        public bool isTimeOver;
    }

    [SerializeField]
    TimeCounter timeCountData = new TimeCounter();

    static TimeCounter timeCounter = new TimeCounter();
    static List<NumberImageRenderer> timeRendererList = new List<NumberImageRenderer>();

	void Start () {
        timeRendererList.Add(GetComponentInChildren<NumberImageRenderer>());
        timeCounter = timeCountData;

        timeCounter.deltaSecond = 0f;
        timeCounter.isTimeOver = false;
    }

    //時間の計測
    static public void UpdateTimeUI()
    {
        if (timeCounter.isTimeOver /*&& !TutorialScript.IsTutorial */ )return;

        timeCounter.deltaSecond += Time.deltaTime;

        double time = timeCounter.maxSecond - timeCounter.deltaSecond;
        foreach (var timeRender in timeRendererList)
        {
            timeRender.Rendering((int)time);
        }

        if (timeCounter.deltaSecond >= timeCounter.maxSecond)
        {
            timeCounter.isTimeOver = true;
        }

    }

    static public bool IsTimeOver()
    {
        return timeCounter.isTimeOver;
    }

}
