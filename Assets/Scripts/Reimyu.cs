using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class Reimyu : MonoBehaviour{

    public GameObject daialogCanvas;
    public GameObject daialogBox;
    public Text daialogeText = null;
    public GameObject unknownName;
    public GameObject reimyuName;
    public GameObject marichaName;
    public Sprite reimyuImageBox;
    public Sprite marichaImageBox;
    public Image textBox;
    public GameObject sankaku;
    public GameObject LastBoss;
    public GameObject VanishWall;//ラスボスステージ左の壁。ラスボス登場で消える。
    [Header("誰が話しているか")]public string [] washa;
    [Header("セリフ内容")][TextArea(1, 3)] public string [] serifu;
    [Header("ボイス")] public AudioClip[] audios;
    [Header("いつから次のメッセージにいけるか。")]public float [] yoin;
    [Header("セリフ番号")] public int serifuNum = -1;


    // Start is called before the first frame update
    void Start(){
        textBox.GetComponent<Image>();
        daialogCanvas.SetActive(true);
        transform.DOMoveX(1430,2.0f); //DoMoveXについてhttps://qiita.com/BEATnonanka/items/b4cca6471e77466cec74
        Invoke("CanNextDaialog", 2.0f);
    }

    // Update is called once per frame
    void Update(){
        if (sankaku.activeSelf) {//sankakuがアクティブならば
            if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.Space)) {//Returnキー=Enterキー
                sankaku.SetActive(false);
                daialogBox.SetActive(false);
                serifuNum++;
                NextMessege();
            }
        }
    }


    void CanNextDaialog() {
        sankaku.SetActive(true);
    }
    void NextMessege() {
        daialogeText.text = serifu[serifuNum];//テキスト変更　参考https://freesworder.net/unity-text-change/
        unknownName.SetActive(false);
        if (washa[serifuNum] == "れいみゅ") {
            textBox.sprite = reimyuImageBox;  //画像切り替え　参考https://futabazemi.net/unity/photo_change_collider/
            marichaName.SetActive(false);
            reimyuName.SetActive(true);
        } else {
            textBox.sprite = marichaImageBox; //オブジェクト切り替え　参考https://dodagon.com/unity/array1
            marichaName.SetActive(true);
            reimyuName.SetActive(false);
        }
        daialogBox.SetActive(true);
        //GameManager.instance.playSE(audios[serifuNum]);
        Invoke("CanNextDaialog", yoin[serifuNum]);
    }
/*
    →おねーしゃー
いもーちょ？

たしゅけーちぇー
→右かられいみゅ登場
ゆゆ、いもーちょ、どうちてこんなみょんなばちょに…。
　お、おやしゃいしゃんが…！
　ゆ？　おやしゃいしゃん？
→ボスセットアクティブオン
→ボス登場(効果音ずーん)
→Idle状態
→おおきすぎるのじぇえええ
ゆんやあああ、おやしゃいしゃんきょわいいいいい
→れいみゅ下に逃げる(これ以降、れいむのコライダーはトリガーとして使用)
→れいみゅの近くを通るたびに、まりちゃを応援
→おねーしゃー、がんばえー！
　きょわいいいいい
*/
}
