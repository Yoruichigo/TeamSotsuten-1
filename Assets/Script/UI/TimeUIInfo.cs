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

    [SerializeField]
    AnimationCurve curve = null;

    [SerializeField]
    Color startColor = Color.white;

    [SerializeField]
    Color endColor = Color.white;

    float playTime = 0;

    static TimeCounter timeCounter = new TimeCounter();
    static List<NumberImageRenderer> timeRendererList = new List<NumberImageRenderer>();
    static Color changeColor = Color.white;

	void Start () {
        timeRendererList.Add(GetComponentInChildren<NumberImageRenderer>());
        timeCounter = timeCountData;

        timeCounter.deltaSecond = 0f;
        timeCounter.isTimeOver = false;
    }

    void Update()
    { 
        float rate = timeCounter.deltaSecond / timeCountData.maxSecond;
        float red = Mathf.Lerp(startColor.r, endColor.r, curve.Evaluate(rate));
        float green = Mathf.Lerp(startColor.g, endColor.g, curve.Evaluate(rate));
        float blue = Mathf.Lerp(startColor.b, endColor.b, curve.Evaluate(rate));

        changeColor = new Color(red, green, blue);
    }

    //時間の計測
    static public void UpdateTimeUI()
    {
        if (timeCounter.isTimeOver)return;

        if (!TutorialSequence.IsTutorial)
        {
            timeCounter.deltaSecond += Time.deltaTime;
        }

        double time = timeCounter.maxSecond - timeCounter.deltaSecond;
        foreach (var timeRender in timeRendererList)
        {
            timeRender.ChnageColor(changeColor);
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
