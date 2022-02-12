using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Title : MonoBehaviour{
    [Header("スタートVoice1")] public AudioClip startVoice1;
    [Header("スタートVoice2")] public AudioClip startVoice2;
    [Header("スタートVoice3")] public AudioClip startVoice3;
    [Header("スタートVoice4")] public AudioClip startVoice4;
    [Header("スタートVoice5")] public AudioClip startVoice5;
    [Header("フェード")] public FadeImage fade;
    private bool firstPush = false;//初めてのプッシュかどうか
    private bool goNextScene = false;

    public void PressStart()
    {
        Debug.Log("Press Start!");
        GameManager.instance.RandomizeSfx(startVoice1, startVoice2, startVoice3, startVoice4, startVoice5);
        

        if (!firstPush)//プッシュ済みではない場合
        {
            fade.StartFadeOut();
            firstPush = true;//一度押すとプッシュ済に
        }
    }

    private void Update() {
        if(!goNextScene && fade.IsFadeOutComplete()) {
            Invoke("NextStage", 2.0f);
        }
    }

    private void NextStage() {
        if(GameManager.instance.stageNum == 99) {
            SceneManager.LoadScene("titleScene");//ステージナンバーが99だとタイトルに戻る。
        }
        else if (GameManager.instance.stageNum > 1) {
            SceneManager.LoadScene("stage" + GameManager.instance.stageNum);
        } else { SceneManager.LoadScene("stage1"); }
        goNextScene = true;
        GameManager.instance.RetryGame();//自分で追加リトライ用にスコア等を初期値に戻す。
    }


    //タイトルへ戻るボタンを押すと、こちらを呼び出す。
    public void GotoTitle() {
        Debug.Log("GotoTitle!");
        GameManager.instance.RandomizeSfx(startVoice1, startVoice2, startVoice3, startVoice4, startVoice5);
        GameManager.instance.stageNum = 99;//ステージナンバーを99にする。


        if (!firstPush)//プッシュ済みではない場合
        {
            fade.StartFadeOut();
            firstPush = true;//一度押すとプッシュ済に
        }
    }

}
