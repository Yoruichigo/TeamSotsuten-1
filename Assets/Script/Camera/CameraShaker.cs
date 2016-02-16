using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CameraShaker : MonoBehaviour
{
    /// <summary>
    /// 振動開始時の振動の大きさ
    /// </summary>
    [SerializeField]
    private float coefShakeIntensity = 50f;

    [SerializeField]
    private float shakeTime = 0.1f;

    uTweenBase[] playList = null;

    void Start()
    {
        playList = uTween.GetPlayList("Shake");
    }

    /// <summary>
    /// 振動させる
    /// </summary>
    public void Shake()
    {

        for (int i = 0; i < playList.Length; i++)
        {
            uTweenMove move = playList[i] as uTweenMove;

            if (move.IsPlaying) continue;

            move.tweenTime = shakeTime;
            move.startPosition = move.cashRectTransform.anchoredPosition3D;

            var randomX = Random.Range(-coefShakeIntensity,coefShakeIntensity);
            var randomY = Random.Range(-coefShakeIntensity,coefShakeIntensity);
            move.targetPosition = move.startPosition + new Vector3(randomX, randomY, 0);

            move.Play();
        }
    }

}
