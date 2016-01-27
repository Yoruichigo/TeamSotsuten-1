/*
 *  回転するスクリプト
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

public class JobRotater : MonoBehaviour {

//    /// <summary>
//    /// 職業のデータ
//    /// </summary>
//    /// 職業のデータはデータベースから貰ってくる
//    [SerializeField]
//    private JobDB JobData = null;
//    /// <summary>
//    /// イージングの種類
//    /// </summary>
//    /// これを変更するとイージングが変わる
//    /// いい感じのに変更してください
//    [SerializeField]
//    private iTween.EaseType easeType = iTween.EaseType.linear;
//    /// <summary>
//    /// 回転にかかる時間
//    /// </summary>
//    /// ここの数字変更すると速度が変わる
//    /// 0に近いほど速い
//    [SerializeField]
//    private float RotationVelocityForSeconds = 5.0f;

//    /// <summary>
//    /// 一回あたりに回転させる角度
//    /// 計算で求めるためインスペクター上から変更しないように
//    /// </summary>
//    private int revolutionAngle = 0;
//    /// <summary>
//    /// 回転する角度
//    /// インスペクター上から変更しないように
//    /// </summary>
//    private float angle = 0.0f;

//    /// <summary>
//    /// 回転しているかどうか
//    /// </summary>
//    private bool isRotate = false;

//    /// <summary>
//    /// 選択中のID
//    /// @changed m_yamada
//    /// </summary>
//    private int selectedID = 0;

//    // Use this for initialization
//    void Start()
//    {
//        Initialization();
//    }

//    // Update is called once per frame
//    private void Update()
//    {
//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            Rotate(-1.0f);
//        }
//        if (Input.GetKeyDown(KeyCode.RightArrow))
//        {
//            Rotate(1.0f);
//        }
//#endif

//    }

//    /// <summary>
//    /// モーションが成功すると、この関数が呼ばれます。
//    /// ここに処理等を記述してください。
//    /// </summary>
//    public void OnMotionComplated()
//    {
//        switch (MotionManager.Instance.MotionSkill)
//        {
//            case MotionManager.MotionSkillType.WEAK:
//                Rotate(1.0f);
//                break;

//            case MotionManager.MotionSkillType.STRENGTH:
//                JobData.Decision(this);
//                break;
//        }
//    }

//    /// <summary>
//    /// 初期化
//    /// </summary>
//    /// 角度計算
//    /// 360°から職業分割るだけ
//    private void Initialization()
//    {
//        revolutionAngle = 360 / JobData.JobTypeCount;
//    }


//    /// <summary>
//    /// 回転
//    /// </summary>
//    /// オイラー角　0～360度
//    /// ラジアン　　0～2π
//    private void Rotate(float dirValue)
//    {
//        /// もし角度が0だったらStart()の時点で呼ばれてない可能性があるので
//        /// いちを呼んでおく
//        if (revolutionAngle == 0)
//        {
//            Initialization();
//        }

//        if (isRotate) return;           //  回転していたら抜ける

//        // @changed m_yamada
//        //  iTween の onupdate で呼ばれている NowRotate() が次のフレームに呼ばれていたため、
//        //  この処理が2回呼ばれていた。
//        //  isRotateをここにtrueにして回避しました。
//        isRotate = true;
//        selectedID += (int)dirValue;
//        JobData.SetSelectJobType(ref selectedID);


//        StartRotate(dirValue);       //  回転方向を決める

//        /// 回転
//        iTween.RotateTo(gameObject, iTween.Hash("y", angle, "time", RotationVelocityForSeconds, "easetype", easeType, "onupdate", "NowRotate", "oncomplete","EndRotate"));
//    }

//    /// <summary>
//    /// 回転中
//    /// </summary>
//    /// フラグをtrueに
//    private void NowRotate()
//    {
//        isRotate = true;
//    }

//    /// <summary>
//    /// 回転終了
//    /// </summary>
//    /// フラグをfalseへ
//    private void EndRotate()
//    {
//        isRotate = false;
//    }

//    /// <summary>
//    /// 回転方向
//    /// </summary>
//    /// 入力された方向をもらってきて回転方向を変える
//    ///  Mathf.RoundToInt()
//    /// ↑四捨五入
//    /// 切り捨てでやったらズレたため四捨五入に変更
//    /// <param name="horizontal">押した方向</param>
//    private void StartRotate(float horizontal)
//    {
//        if (horizontal > 0)
//        {
//            angle = Mathf.RoundToInt(gameObject.transform.localEulerAngles.y) - revolutionAngle;
//        }
//        else if(horizontal < 0)
//        {
//            angle = Mathf.RoundToInt(gameObject.transform.localEulerAngles.y) + revolutionAngle;
//        }
//        angle = Mathf.RoundToInt(angle);
//    }

}
