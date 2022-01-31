using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Title : MonoBehaviour{

    [Header("フェード")] public FadeImage fade;
    private bool firstPush = false;//初めてのプッシュかどうか
    private bool goNextScene = false;

    public void PressStart()
    {
        Debug.Log("Press Start!");
        

        if (!firstPush)//プッシュ済みではない場合
        {
            fade.StartFadeOut();
            firstPush = true;//一度押すとプッシュ済に
        }
    }

    private void Update() {
        if(!goNextScene && fade.IsFadeOutComplete()) {
            SceneManager.LoadScene("stage1");
            goNextScene = true;
            GameManager.instance.RetryGame();//自分で追加リトライ用にスコア等を初期値に戻す。
        }
    }
}
