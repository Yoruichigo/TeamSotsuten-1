/*
 *  職業を生成するスクリプト
 * 
 *  決め事
 * 
 *  命名規則：   Pascal形式　例) AttackCount; Camel形式　attackCount
 *      名前空間 Pascal形式　クラス、構造体　Pascal形式　プロパティ　Pascal形式
 *      メンバ変数(フィールド)　Camel形式　メソッド　Pascal形式　パラメータ　Camel形式
 *      enum型   Pascal形式　
 *      enum（中身）、定数 「大文字」と「単語の区切りにアンダーバー」の組み合わせで命名する。
 *      例) ATTACK_COUNT_MAX
 *      
 *  メソット    1メソッド10行以内　最大2インデント　名前をわかりやすく
 *  Property    getのみ行う　setは、プライベート
 * 
 *  SendMessageを使わない　Editorから読み込むだけなら[Serialize Failed]を使用する
 * 
 *  状態管理をしっかり行う　ジェネリック思考で考える
 *  ほぼ全てに、コメントを書く
 *  多重ループを使用する場合、分かりやすい単語にする
 *
 * 
 * Code by shinnnosuke hiratsuka
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JobGeneration : MonoBehaviour {


    ///// <summary>
    ///// 半径
    ///// </summary>
    ///// 自分からの距離
    //[SerializeField]
    //private float radius = 5.0f;

    ///// <summary>
    ///// 角度
    ///// </summary>
    ///// 生成した時の間隔
    //private float angle = 0.0f;

    ///// <summary>
    ///// 職業のデータ
    ///// </summary>
    ///// 職業のデータはデータベースから貰ってくる
    //[SerializeField]
    //private JobDB JobData = null;

    ///// <summary>
    ///// 確定位置
    ///// </summary>
    ///// 現状特に意味は無い
    //[SerializeField]
    //private GameObject fixingPosition = null;

    ///// <summary>
    ///// Startより先呼ばれる奴
    ///// </summary>
    ///// 生成角度θの計算
    //void Awake()
    //{
    //    angle = 360/JobData.JobTypeCount;
    //}

    //// Use this for initialization
    //void Start ()
    //{
    //    Generation();

    //    FixingPosition();
    //}
    ///// <summary>
    ///// 生成
    ///// </summary>
    //private void Generation()
    //{
    //    /// 職業の数だけぶん回し
    //    for (int i = 0; i < JobData.JobTypeCount; i++)
    //    {
    //        GameObject clone = (GameObject)Instantiate(JobData.GetJobDataFindArray(i).job);     /// 生成
    //        clone.name = JobData.GetJobDataFindArray(i).job.name;                               /// 名前変更
    //        clone.transform.SetParent(gameObject.transform);                                    /// 親変更
    //        clone.transform.position = GeneratedPosition(i);                                    /// 座標変更
    //        clone.transform.Rotate(GeneratedAngle(i));                                          /// 回転変更
    //        clone.transform.localScale = JobData.GetJobDataFindArray(i).job.transform.lossyScale;

    //    }

    //    JobData.SetSelectJobType(0);
    //}


    ///// <summary>
    ///// 生成場所を返す
    ///// </summary>
    ///// <param name="array">何個目か</param>
    ///// <returns>座標</returns>
    //private Vector3 GeneratedPosition(int array)
    //{
    //    //  座標
    //    Vector3 pos = Vector3.zero;

    //    /// 角度
    //    float ang = 0;

    //    ang = (Mathf.PI * angle * array) / 180;     /// 角度求めて

    //    pos.x = -radius * Mathf.Sin(ang);           /// 座標入れて
    //    pos.y = gameObject.transform.position.y;
    //    pos.z = -radius * Mathf.Cos(ang);

    //    return pos;

    //}
    ///// <summary>
    ///// 生成した時に目指すべき方向を見据える
    ///// </summary>
    ///// <param name="array">何人目か</param>
    ///// <returns>未来</returns>
    //private Vector3 GeneratedAngle(int array)
    //{
    //    //  座標
    //    Vector3 ang = Vector3.zero;

    //    /// 角度求めて
    //    /// 正面が未来になるよに角度を反転させる
    //    ang = new Vector3(0, (angle * array) + 180,0);

    //    return ang;
    //}

    ///// <summary>
    ///// 確定位置の変更
    ///// </summary>
    //private void FixingPosition()
    //{
    //    fixingPosition.transform.position = new Vector3(0, 0, -radius);
    //}
}
