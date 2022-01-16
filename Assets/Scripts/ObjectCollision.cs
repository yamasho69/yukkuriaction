using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//レッスン45で作成
public class ObjectCollision : MonoBehaviour{

    [Header("これを踏んだ時のプレイヤーが跳ねる高さ")] public float boundHeight;

    ///<summary>
    ///このオブジェクトをプレイヤーが踏んだかどうか
    /// </summary>
    [HideInInspector]public bool playerStepOn;//HideInspectorはインスペクターで入力できないようにする。他のクラスからは呼び出せる。

}
