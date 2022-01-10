using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Title : MonoBehaviour{

    private bool firstPush = false;//初めてのプッシュかどうか

    public void PressStart()
    {
        Debug.Log("Press Start!");

        if (!firstPush)//プッシュ済みではない場合
        {
            Debug.Log("Go Next Scene");
            //ここに次のシーンへ行く命令を書く

            firstPush = true;//一度押すとプッシュ済に
        }
    }
}
