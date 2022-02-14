using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//レッスン47で作成
public class FadeImage : MonoBehaviour{

    [Header("最初からフェードインが完了しているかどうか")] public bool firstFadeInComp;

    private Image img = null;
    private int frameCount = 0;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private float timer = 0.0f;
    private bool compFadeIn = false;
    private bool compFadeOut = false;


    public void StartFadeIn() {
        if(fadeIn || fadeOut) {
            return;
        }
        fadeIn = true;
        compFadeIn = false;
        timer = 0.0f;
        img.color = new Color(1, 1, 1, 1);
        img.fillAmount = 1;
        img.raycastTarget = true;
    }

    public bool IsFadeInComplete() {
        return compFadeIn;
    }

    public void StartFadeOut() {
        if (fadeIn || fadeOut) {
            return;
        }
        fadeOut = true;
        compFadeOut = false;
        timer = 0.0f;
        img.color = new Color(1, 1, 1, 1);
        img.fillAmount = 1;
        img.raycastTarget = true;
    }

    public bool IsFadeOutComplete() {
        return compFadeOut;
    }

    // Start is called before the first frame update
    void Start(){
        //試しにフェードイン
        img = GetComponent<Image>();
        if (firstFadeInComp) {
            FadeInComplete();
        } else {
            StartFadeIn();
        }
    }

    // Update is called once per frame
    void Update(){
        if (frameCount > 2) {
            if (fadeIn) {
                FadeInUpdate();
            }else if (fadeOut) {
                FadeOutUpdate();
            }
        }
        ++frameCount;
    }

    private void FadeInUpdate() {
        //フェード中
        if (timer < 1f) {
            img.color = new Color(1, 1, 1, 1 - timer);
            img.fillAmount = 1 - timer;
        }
        //フェード完了
        else {
            FadeInComplete();
        }
        timer += Time.deltaTime;
    }

    private void FadeInComplete() {
        img.color = new Color(1, 1, 1, 0);
        Time.timeScale = 1;
        img.fillAmount = 0;
        img.raycastTarget = false;
        timer = 0.0f;
        fadeIn = false;
        compFadeIn = true;
    }
    
    private void FadeOutUpdate() {
        //フェード中
        if (timer < 1f) {
            img.color = new Color(1, 1, 1, timer);
            img.fillAmount = timer;
        }
        //フェード完了
        else {
            FadeOutComplete();
        }
            timer += Time.deltaTime;
    }

    private void FadeOutComplete() {
        img.color = new Color(1, 1, 1, 1);
        img.fillAmount = 1;
        img.raycastTarget = false;
        timer = 0.0f;
        fadeOut = false;
        compFadeOut = true;

    }
}
