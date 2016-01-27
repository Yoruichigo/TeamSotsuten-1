//-------------------------------------------------------------
// サーバーから送られたデータを処理するクラス
//
// code by Gai Takakura
//-------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CliantEnemySpawner : MonoBehaviour {
    // エネミーとなるオブジェクト
    public List<GameObject> enemies = new List<GameObject>();

    private int useNumber;

    void Start()
    {
        // 子オブジェクトを全部取得する
        foreach (Transform child in transform)
        {
            enemies.Add(child.gameObject);
        }
    }

    /// <summary>
    /// 発生
    /// </summary>
    public void Spawn(int num)
    {
        if (enemies.Count < num)
        {
            Debug.Log("要素外です");
            return;
        }

        useNumber = num;

        var hash = new Hashtable();
        {
            hash.Add("scale", new Vector3(1f, 1f, 1f)); // 設定するサイズ
            hash.Add("time", 1f);                       // 1秒で行う
            hash.Add("easetype", "easeOutQuad");        // イージングタイプを設定
            hash.Add("onstart", "ChangeActive");        // 初めにメソッドを呼ぶ
        }
        iTween.ScaleTo(enemies[useNumber], hash);
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead(int num)
    {
        if (enemies.Count < num)
        {
            Debug.Log("要素外です");
            return;
        }

        useNumber = num;

        var hash = new Hashtable();
        {
            hash.Add("scale", new Vector3(0f, 0f, 0f)); // 設定するサイズ
            hash.Add("time", 1f);                       // 1秒で行う
            hash.Add("easetype", "easeOutQuad");        // イージングタイプを設定
            hash.Add("oncomplete", "ChangeActive");     // 最後にメソッドを呼ぶ
        }
        iTween.ScaleTo(enemies[useNumber], hash);
    }

    /// <summary>
    /// アクティブ状態を変更する
    /// </summary>
    void ChangeActive()
    {
        if (!enemies[useNumber].activeInHierarchy)
            enemies[useNumber].SetActive(true);
        else
            enemies[useNumber].SetActive(false);
    }
}
