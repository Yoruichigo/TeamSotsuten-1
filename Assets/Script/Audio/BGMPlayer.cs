// -------------------------------------------
//  BGMを再生するスクリプト
//
//  code by m_yamada
// -------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct FadeTimeData
{
    public FadeTimeData(float inTime, float outTime)
        : this()
    {
        this.inTime = inTime;
        this.outTime = outTime;
    }

    public static FadeTimeData Zero { get { return new FadeTimeData(0, 0); } }

    public float inTime;
    public float outTime;
}


public class BGMPlayer : Singleton<BGMPlayer>
{

    public class Data
    {
        public Data(BGMAudioData data)
        {
            this.data = data;
            clip = Resources.Load("BGM/" + data.label) as AudioClip;
        }

        public BGMAudioData data;
        public AudioClip clip;
    }

    const float minVolume = 0;
    const float maxVolume = 0.7f;
    const float startFadeInVolume = 0.005f;

    AudioSource source = null;
    Dictionary<Audio.BGMID, Data> audioMap = new Dictionary<Audio.BGMID, Data>();
    FadeTimeData FadeTime;

    public bool IsPlaying { get { return source.isPlaying; } }

    public override void Awake() 
    {
        base.Awake();
        source = GetComponent<AudioSource>();

        foreach (var data in AudioManager.BGMData)
        {
            audioMap.Add(data.id, new Data(data));
        }

    }

    public override void Start()
    {
        base.Start();

	}
	
    public override void Update()
    {
        base.Update();

	}

    /// <summary>
    /// 名前で再生中かどうかを探す
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public bool IsPlayingToName(Audio.BGMID id)
    {
        if (source.clip == null) return false;

        if (source.clip.name == audioMap[id].clip.name && source.isPlaying)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <param name="id">Resources/BGM/の中にあるオーディオ名</param>
    /// <param name="fadeInTime">フェードイン時間</param>
    public void Play(Audio.BGMID id, FadeTimeData fadeTime)
    {
        if (SequenceManager.Instance.IsBuildWatch) return;
        
        FadeTime = fadeTime;
        source.clip = audioMap[id].clip;
        source.Play();
        source.volume = startFadeInVolume;
        StartFadeIn(FadeTime.inTime);
    }

    /// <summary>
    /// 停止
    /// </summary>
    /// <param name="fadeOutTime">フェードアウト時間</param>
    public void Stop()
    {
        StartFadeOut(FadeTime.outTime);
    }

    /// <summary>
    /// フェードアウトスタート
    /// </summary>
    /// <param name="time">時間</param>
    void StartFadeOut(float time)
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", maxVolume, "to", minVolume, "time", time, "onupdate", "UpdateHandler"));
    }

    /// <summary>
    /// フェードインスタート
    /// </summary>
    /// <param name="time">時間</param>
    void StartFadeIn(float time)
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", startFadeInVolume, "to", maxVolume, "time", time, "onupdate", "UpdateHandler"));
    }

    void UpdateHandler(float value)
    {
        source.volume = value;

        if (source.volume <= 0)
        {
            source.Stop();
        }
    }

}


/*
 * <BGMのResource名リスト>
 * GameBGM
 * 
 * 
 * 
 * 
*/