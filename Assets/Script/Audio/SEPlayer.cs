// -------------------------------------------
//  SEを再生するスクリプト
//
//  code by m_yamada
// -------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SEPlayer : Singleton<SEPlayer>
{
    public class Data
    {
        public Data(SEAudioData data)
        {
            this.data = data;
            clip = Resources.Load("SE/" + data.label) as AudioClip;
        }
        public SEAudioData data;
        public AudioClip clip;
    }

    List<AudioSource> sources = new List<AudioSource>();
    Dictionary<Audio.SEID, Data> audioMap = new Dictionary<Audio.SEID, Data>();

    public const float maxVolume = 1.0f;


    void Awake()
    {
        base.Awake();

        foreach (var data in AudioManager.SEData)
        {
            audioMap.Add(data.id, new Data(data));
        }

    }

    void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        foreach (var source in sources)
        {
            if (!source.isPlaying)
            {
                Destroy(source);
                sources.Remove(source);
                break;
            }
        }
    }


    /// <summary>
    /// 再生
    /// </summary>
    /// <param name="resName">Resource名</param>
    public void Play(Audio.SEID id, float pitch = 1.0f, bool loop = false)
    {
        if (SequenceManager.Instance.IsBuildWatch) return;
        
        sources.Add(gameObject.AddComponent<AudioSource>());
        var index = sources.Count - 1;
        sources[index].clip = audioMap[id].clip;
        sources[index].pitch = pitch;
        sources[index].loop = loop;
        sources[index].volume = maxVolume;
        sources[index].Play();
    }

    /// <summary>
    /// 音量を変える
    /// </summary>
    /// <param name="resName"></param>
    /// <param name="volume"></param>
    public void ChangeVolume(Audio.SEID id, float volume)
    {
        foreach (var source in sources)
        {
            if (source.clip.name == audioMap[id].clip.name)
            {
                source.volume = volume;
                break;
            }
        }
    }


    /// <summary>
    /// 停止
    /// </summary>
    /// <param name="resName">Resource名</param>
    public void Stop(Audio.SEID id, float time = 0.0f)
    {
        StartCoroutine(WaitStop(id, time));
    }

    /// <summary>
    /// 停止
    /// </summary>
    /// <param name="resName">Resource名</param>
    IEnumerator WaitStop(Audio.SEID id, float time)
    {
        yield return new WaitForSeconds(time);
        
        foreach (var source in sources)
        {
            if (source.clip.name == audioMap[id].clip.name)
            {
                source.Stop();
                break;
            }
        }

    }


    /// <summary>
    /// すべて停止
    /// </summary>
    /// <param name="resName">Resource名</param>
    public void AllStop()
    {
        foreach (var source in sources)
        {
            source.Stop();
        }
    }

    /// <summary>
    /// 再生中かどうか
    /// </summary>
    /// <param name="resName">Resource名</param>
    /// <returns></returns>
    public bool IsPlaying(Audio.SEID id)
    {
        foreach (var source in sources)
        {
            if (source.isPlaying && source.clip.name == audioMap[id].clip.name)
            {
                return true;
            }
        }

        return false;
    }

}



/*
 * <SEのResource名リスト>
 * EnemyAppearance  : 敵の登場
 * EnemyAttack      : 敵の攻撃
 * EnemyHit         : 敵がヒットした音
 * EnemySiren       : 敵の出現ワーニング
 * Decision         : 決定
 * PlayerWeakAttack     : プレイヤー攻撃
 * PlayerStrengthAttack : プレイヤー攻撃
 * Damage           : ダメージ
*/