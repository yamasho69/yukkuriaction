using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

//レッスン56で作成
public class ContinuePoint : MonoBehaviour {
    [Header("コンティニュー番号")] public int continueNum;
    [Header("声")] public AudioClip voice;
    [Header("音")] public AudioClip se;
    [Header("プレイヤー判定")] public PlayerTriggerCheck trigger;
    [Header("スピード")] public float speed = 2.0f;
    [Header("取得アニメーション")] public AnimationCurve curve;
    [Header("変更前ダンボール")]public Image beforeBox;
    [Header("変更後ダンボール")]public Sprite afterBox;

    private bool on;
    private float timer;

    // Start is called before the first frame update
    void Start() {
        if (trigger == null || se == null) {
            Debug.Log("インスペクターの設定が足りません");
            Destroy(this);
        }

    }

    // Update is called once per frame
    void Update() {
        if (trigger.isOn && !on) {
            GameManager.instance.continueNum = continueNum;
            GameManager.instance.playSE(voice);
            GameManager.instance.playSE(se);
            beforeBox.sprite = afterBox;
            on = true;
        }
        if (on) {
            //if(timer < 1.0f) {
            //transform.localScale = Vector3.one * curve.Evaluate(timer);
            //timer += speed * Time.deltaTime;
           
            Invoke("Rotate", 1.5f);
            //} else {
                //transform.localScale = Vector3.one * curve.Evaluate(1.0f);
                //gameObject.SetActive(false);
                //on = false;
            //}
        }
    }

    void Rotate() {
        this.transform.DORotate(Vector3.right * 90f, 1.5f);
        Invoke("Vanish", 1.5f);
    }

    void Vanish(){
        gameObject.SetActive(false);
        on = false;
    }
}
