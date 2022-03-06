using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


//一時停止ボタン、参考　https://www.youtube.com/watch?v=w10_AXiGYuY
public class PauseScript : MonoBehaviour{

    bool IsOnPause;
    public Sprite playButton;
    public Sprite pauseButton;
    public GameObject goToTitleButton;
    public GameObject pauseEffect;
    public AudioClip pauseOnSE;
    public AudioClip pauseOffSE;
    public GameObject joyStick;
    public GameObject jumpButton;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.P) && GameManager.instance.isGameOver == false) {//GetKeyDown関数にしないと何回も押せてしまう。Pキーでもポーズがかかるように改良
            pauseTheGame();
        }
    }

    public void pauseTheGame(){
        if (IsOnPause) {
            Time.timeScale = 1;
            IsOnPause = false;
            //this.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            //押すとボタンの画像自体が変わる方式に変更　https://futabazemi.net/unity/photo_change_collider/
            this.gameObject.GetComponent<Image>().sprite = pauseButton;
            GameManager.instance.playSE(pauseOffSE);
            pauseEffect.SetActive(false);
            joyStick.SetActive(true);
            jumpButton.SetActive(true);
            goToTitleButton.SetActive(false);
        } else {
            Time.timeScale = 0;
            IsOnPause = true;
            //this.gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            this.gameObject.GetComponent<Image>().sprite = playButton;
            GameManager.instance.playSE(pauseOnSE);
            pauseEffect.SetActive(true);
            goToTitleButton.SetActive(true);
            jumpButton.SetActive(false);
            joyStick.SetActive(false);
        }
    }
}
