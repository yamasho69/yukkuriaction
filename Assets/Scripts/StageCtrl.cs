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
    [Header("フェード")] public FadeImage fade;//51
    [Header("ゲームオーバーSE")]public AudioClip gameOverSE;
    [Header("リトライSE")]public AudioClip retrySE;
    [Header("ステージクリアSE")]public AudioClip stageClearSE;
    [Header("ステージクリア")]public GameObject stageClearObj;
    [Header("ステージクリア判定")]public PlayerTriggerCheck stageClearTrigger;

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
        if (playerObj != null && continuePoint != null && continuePoint.Length > 0 
            && gameOverObj != null && fade !=null) 
        {
            gameOverObj.SetActive(false);
            stageClearObj.SetActive(false);//56
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
            doGameOver = true;
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
    }
}
