// code by Gai Takakura
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// GameObjectを非Active状態でPoolingするクラス
public class ObjectPool : MonoBehaviour {
    private static ObjectPool instance = null;  // 自身のポインタ
    public static ObjectPool Instance           // ポインタへのアクセス
    {
        get {
            if (instance == null)
            {
                // シーン上から取得する
                instance = FindObjectOfType<ObjectPool>();

                if (instance == null)
                {
                    // ゲームオブジェクトを作成しObjectPoolコンポーネントを追加する
                    instance = new GameObject("ObjectPool").AddComponent<ObjectPool>();
                }
            }
            return instance;
        }
    }

    // オブジェクトプール
	Dictionary<int, List<GameObject>> pooledObject = new Dictionary<int, List<GameObject>>();

    /// <summary>
    /// 新しいプールを生成する
    /// </summary>
    /// <param name="prefab">プーリングするオブジェクト</param>
    /// <param name="createCount">プーリングする数</param>
    public void SetObject(GameObject prefab, int createCount)
    {
        // 連想配列にセット
        pooledObject.Add(prefab.GetInstanceID(), new List<GameObject>());

        // プーリング
        for (int i = 0; i < createCount; i++)
        {
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            pooledObject[prefab.GetInstanceID()].Add(obj);
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// プールからオブジェクトを取得する
    /// </summary>
    /// <param name="prefab">プーリングしたゲームオブジェクト</param>
    /// <param name="position">セットする座標</param>
    /// <param name="rotation">セットする回転</param>
    /// <param name="parent">parentに設定するGameObjectのTransform</param>
    /// <returns>Active状態にしたゲームオブジェクト</returns>
    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        // キーが存在しない
        if (!pooledObject.ContainsKey(prefab.GetInstanceID()))
        {
            return null;
        }

        // プールを取得
        List<GameObject> pool = pooledObject[prefab.GetInstanceID()];

        // 既に生成した分で足りている場合
        foreach (var list in pool)
        {
            if (!list.activeInHierarchy)
            {
                list.transform.position = position;
                list.transform.rotation = rotation;
                list.transform.parent = parent;
                list.SetActive(true);
                return list;
            }
        }

        // 不足していた場合
        var obj = Instantiate(prefab, position, rotation) as GameObject;
        obj.transform.parent = parent;
        pool.Add(obj);

        return obj;
    }

    /// <summary>
	/// プールを取得する
	/// </summary>
	/// <returns>The list.</returns>
	/// <param name="ownerID"> プールのキー </param>
	public List<GameObject> GetList(GameObject prefab)
    {
        // キーが存在しない
        if (!pooledObject.ContainsKey(prefab.GetInstanceID()))
        {
            return null;
        }

        return pooledObject[prefab.GetInstanceID()];
    }

    /// <summary>
    /// オブジェクトプールを破棄
    /// </summary>
    /// <param name="prefab">プーリングしたゲームオブジェクト</param>
    public void ReleasePool(GameObject prefab)
    {
        // キーが存在しない
        if (!pooledObject.ContainsKey(prefab.GetInstanceID()))
        {
            return;
        }

        // プールを取得
        List<GameObject> pool = pooledObject[prefab.GetInstanceID()];

        // オブジェクトを破棄
        foreach (var list in pool)
        {
            Destroy(list);
        }
        pool.Clear();

        pooledObject.Remove(prefab.GetInstanceID());
    }

    /// <summary>
    /// GameObjectを非アクティブにする
    /// </summary>
    /// <param name="obj">Object.</param>
    public void ReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
