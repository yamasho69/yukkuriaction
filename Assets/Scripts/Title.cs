using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Title : MonoBehaviour{
    [Header("リトライVoice1")] public AudioClip retryVoice1;
    [Header("リトライVoice2")] public AudioClip retryVoice2;
    [Header("リトライVoice3")] public AudioClip retryVoice3;
    [Header("リトライVoice4")] public AudioClip retryVoice4;
    [Header("リトライVoice5")] public AudioClip retryVoice5;
    [Header("フェード")] public FadeImage fade;
    private bool firstPush = false;//初めてのプッシュかどうか
    private bool goNextScene = false;

    public void PressStart()
    {
        Debug.Log("Press Start!");
        GameManager.instance.RandomizeSfx(retryVoice1, retryVoice2, retryVoice3, retryVoice4, retryVoice5);


        if (!firstPush)//プッシュ済みではない場合
        {
            fade.StartFadeOut();
            firstPush = true;//一度押すとプッシュ済に
        }
    }

    private void Update() {
        if(!goNextScene && fade.IsFadeOutComplete()) {
            Invoke("NextStage", 1.5f);
        }
    }

    private void NextStage() {
        SceneManager.LoadScene("stage1");
        goNextScene = true;
        GameManager.instance.RetryGame();//自分で追加リトライ用にスコア等を初期値に戻す。
    }
}
