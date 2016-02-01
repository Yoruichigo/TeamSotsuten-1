using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class uTweenColor : uTweenBase {

    public Color startColor = Color.white;
    public Color targetColor = Color.white;
    public Image cashImage = null;
    public SpriteRenderer cashSpriteRenderer = null;
    Color tweenStartColor = Color.white;
    bool isImage = true;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init()
    {
        cashImage = GetComponent<Image>();

        if (cashImage == null)
        {
            cashSpriteRenderer = GetComponent<SpriteRenderer>();

            if (cashSpriteRenderer == null)
            {
                Debug.Log("Image or SpriteRenderer がありません。");
                Destroy(this);
                return;
            }

            isImage = false;
        }

        if (isImage)
        {
            cashImage.color = startColor;
        }
        else
        {
            cashSpriteRenderer.color = startColor;
        }

        tweenStartColor = startColor;

    }

    void Update()
    {
        if (!isTweening) return;

        var curve = GetCurve();
        var red = Mathf.Lerp(tweenStartColor.r, targetColor.r, curve);
        var green = Mathf.Lerp(tweenStartColor.g, targetColor.g, curve);
        var blue = Mathf.Lerp(tweenStartColor.b, targetColor.b, curve);
        var alpha = Mathf.Lerp(tweenStartColor.a, targetColor.a, curve);

        if (isImage)
        {
            cashImage.color = new Color(red, green, blue, alpha);
        }
        else
        {
            cashSpriteRenderer.color = new Color(red, green, blue, alpha);
        }

        Finish();
    }
}
