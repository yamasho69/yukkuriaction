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
    public bool goNextScene = false;
    public GameObject maricha;

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
        if(GameManager.instance.stageNum == 0) {
            SceneManager.LoadScene("titleScene");//ステージナンバーが0だとタイトルに戻る。
        }
        else if (GameManager.instance.stageNum > 1) {
            SceneManager.LoadScene("stage" + GameManager.instance.stageNum);
        } else { SceneManager.LoadScene("stage1"); }
        goNextScene = true;
        GameManager.instance.RetryGame();//自分で追加リトライ用にスコア等を初期値に戻す。
    }


    //タイトルへ戻るボタンを押すと、こちらを呼び出す。
    public void GotoTitle() {
        
        GameManager.instance.RandomizeSfx(startVoice1, startVoice2, startVoice3, startVoice4, startVoice5);
        if (!firstPush)//プッシュ済みではない場合
        {
            Debug.Log("GotoTitle!");
            fade.StartFadeOut();
            GameManager.instance.stageNum = 0;//ステージナンバーを0にする。
            firstPush = true;//一度押すとプッシュ済に
            if(Time.timeScale == 0) {//超重要。ポーズ中はFadeimageを動かしているtimerも止まってしまう。
                //なのでTime.timeScaleを1に戻す必要がある。
                //しかし、穴やトラップの上で時間を戻すと、Playerが反応してしまう。
                //なので、Player、さらにリジッドボディとコライダーを破壊し、反応しないようにしてから
                //Time.timeScaleを元に戻す。非アクティブ化するだけでは、OnCollisionやOnTriggerが反応してしまう。
                //参考https://qiita.com/OKsaiyowa/items/9579ac348ac860cd522e
                //参考https://atsushishi.xyz/2017/12/deltatime_realtime/
                Destroy(maricha.GetComponent<Player>());
                Time.timeScale = 1;
                Destroy(maricha.GetComponent<Rigidbody2D>());
                Destroy(maricha.GetComponent<CapsuleCollider2D>());
            }
        }
    }
}
