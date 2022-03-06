using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//レッスン50で作成
public class StageCtrl : MonoBehaviour{

    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;
    [Header("コンテニュー位置")] public GameObject[] continuePoint;
    [Header("ゲームオーバー")] public GameObject gameOverObj;//51
    [Header("タイトルに戻るボタン")] public GameObject goToTitleObj;//ポーズ画面でも使用するため、ゲームオーバーの子オブジェクトから分離
    [Header("フェード")] public FadeImage fade;//51
    [Header("ステージスタートVoices")] public AudioClip [] startVoices;
    [Header("ゲームオーバーVoices")] public AudioClip [] gameOverVoices;
    [Header("ステージクリアVoices")] public AudioClip [] stageClearVoices;
    [Header("ステージスタートVoice1")] public AudioClip startVoice1;
    [Header("ステージスタートVoice2")] public AudioClip startVoice2;
    [Header("ステージスタートVoice3")] public AudioClip startVoice3;
    [Header("ステージスタートVoice4")] public AudioClip startVoice4;
    [Header("ステージスタートVoice5")] public AudioClip startVoice5;
    [Header("ゲームオーバーVoice1")] public AudioClip gameOverVoice1;
    [Header("ゲームオーバーVoice2")] public AudioClip gameOverVoice2;
    [Header("ゲームオーバーVoice3")] public AudioClip gameOverVoice3;
    [Header("ゲームオーバーVoice4")] public AudioClip gameOverVoice4;
    [Header("ゲームオーバーVoice5")] public AudioClip gameOverVoice5;
    [Header("ステージクリアSE")]public AudioClip stageClearSE;
    [Header("ステージクリアVO1")]public AudioClip stageClearVoice1;
    [Header("ステージクリアVO2")]public AudioClip stageClearVoice2;
    [Header("ステージクリアVO3")]public AudioClip stageClearVoice3;
    [Header("ステージクリア")]public GameObject stageClearObj;
    [Header("ステージクリア判定")]public PlayerTriggerCheck stageClearTrigger;
    [Header("ジャンプボタン")] public GameObject jumpButton;
    [Header("ジョイスティック")] public GameObject joyStick;

    private Player p;
    //以下の変数は51で追加
    private int nextStageNum;
    private bool startFade = false;
    private bool doGameOver = false;
    private bool retryGame = false;
    private bool doSceneChange = false;
    private bool doClear = false;
    
    // Start is called before the first frame update
    void Start(){
        GameManager.instance.RandomizeSfx(startVoices);
        if (playerObj != null && continuePoint != null && continuePoint.Length > 0 
            && gameOverObj != null && fade !=null) 
        {
            gameOverObj.SetActive(false);
            goToTitleObj.SetActive(false);
            stageClearObj.SetActive(false);//56
            jumpButton.SetActive(true);
            joyStick.SetActive(true);
            playerObj.transform.position = continuePoint[0].transform.position;

            p = playerObj.GetComponent<Player>();
            if(p == null) {
                Debug.Log("プレイヤーじゃないものがアタッチされているよ。");
            }
        } 
        else 
        {
            Debug.Log("設定が足りていないよ！");
        }
    }

    // Update is called once per frame
    void Update(){
        if(GameManager.instance.isGameOver && !doGameOver) {
            gameOverObj.SetActive(true);
            goToTitleObj.SetActive(true);
            jumpButton.SetActive(false);
            joyStick.SetActive(false);
            doGameOver = true;
            GameManager.instance.RandomizeSfx(gameOverVoices);
        }
        else if(p!= null && p.IsContinueWaiting() && !doGameOver) {
            if(continuePoint.Length > GameManager.instance.continueNum) {
                playerObj.transform.position = continuePoint[GameManager.instance.continueNum].transform.position;
                p.ContinuePlayer();
            } else 
            {
                Debug.Log("コンティニューポイントの設定が足りてないよ！");
            }
        }else if(stageClearTrigger != null && stageClearTrigger.isOn && !doGameOver && !doClear) {//56
            StageClear();
            doClear = true;
        }




        if(fade != null && startFade && !doSceneChange) {
            if (fade.IsFadeOutComplete()) {
                if (retryGame) {
                    GameManager.instance.RetryGame();
                } else {
                    GameManager.instance.stageNum = nextStageNum;
                }
                GameManager.instance.isStageClear = false;//56で追加。クリアフラグ。
                //コンティニューナンバーをリセットする必要があるか？
                SceneManager.LoadScene("stage" + nextStageNum);
                doSceneChange = true;
            }
        }
    }

    public void Retry() {
        ChangeScene(1);
        retryGame = true;
    }

    public void ChangeScene(int num) {
        if(fade != null) {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
        }
    }

    //56で作成
    public void StageClear() {
        GameManager.instance.isStageClear = true;
        stageClearObj.SetActive(true);
        GameManager.instance.playSE(stageClearSE);
        GameManager.instance.continueNum = 0;//コンティニューポイントを0番に戻す。
        GameManager.instance.RandomizeSfx(stageClearVoices);
    }
}
