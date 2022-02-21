using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Cinemachine;

public class Reimyu : MonoBehaviour {

    public GameObject daialogCanvas;
    public GameObject daialogBox;
    public Text daialogeText = null;
    public GameObject unknownName;
    public GameObject reimyuName;
    public GameObject marichaName;
    public GameObject niyunName;
    public Sprite reimyuImageBox;
    public Sprite marichaImageBox;
    public Sprite niyunImageBox;
    public GameObject movingPlatformRight;
    public Image textBox;
    public GameObject sankaku;
    public GameObject lastBoss;
    public Enemy_Bullet_Boss enemy_Bullet_Boss;
    public Player player;
    public GameObject maricha;
    public GameObject vanishWall;//ラスボスステージ左の壁。ラスボス登場で消える。
    public Animator marichaAnimator;//まりちゃのアニメーション。参考https://teratail.com/questions/62903
    private Animator animator;
    public GameObject camera1;
    public GameObject camera2;
    private AudioSource audioSource;
    private BoxCollider2D boxCollider2;

    public bool afterBoss;//ボスを倒しているか。

    [Header("誰が話しているか")] public string[] washa;
    [Header("セリフ内容")] [TextArea(1, 3)] public string[] serifu;
    [Header("ボイス")] public AudioClip[] audios;
    [Header("いつから次のメッセージにいけるか。")] public float[] yoin;
    [Header("セリフ番号")] public int serifuNum = -1;//UpDateが始まると、キーを押すたびに増える。デフォルト値(0)のままだと、配列が要素1から始まってしまうため、要素0を使用するため－１とする。
    [Header("応援Voice1")] public AudioClip ouenVoice1;
    [Header("応援Voice2")] public AudioClip ouenVoice2;
    [Header("応援Voice3")] public AudioClip ouenVoice3;
    [Header("応援Voice4")] public AudioClip ouenVoice4;
    [Header("応援Voice5")] public AudioClip ouenVoice5;
    [Header("逃げるSE")] public AudioClip escapeSE;
    [Header("フェード")] public FadeImage fade;//51
    [Header("エンディングでカメラ追従する点")] public Transform follow;

    //以下エンディングのセリフの配列
    [Header("誰が話しているか")] public string[] ending_washa;
    [Header("セリフ内容")] [TextArea(1, 3)] public string[] ending_serifu;
    [Header("ボイス")] public AudioClip[] ending_audios;
    [Header("いつから次のメッセージにいけるか。")] public float[] ending_yoin;
    [Header("セリフ番号")] public int ending_serifuNum = 0;

    [Header("ダイコングループ")] public GameObject daicons;
    [Header("ダイコン１")] public GameObject daicon01;
    [Header("ダイコン２")] public GameObject daicon02;


    // Start is called before the first frame update
    void Start() {
        textBox.GetComponent<Image>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        boxCollider2 = GetComponent<BoxCollider2D>();
        daialogCanvas.SetActive(true);
        transform.DOMoveX(1435, 2.0f); //DoMoveXについてhttps://qiita.com/BEATnonanka/items/b4cca6471e77466cec74
        Invoke("CanNextDaialog", 2.0f);
    }

    // Update is called once per frame
    void Update() {
        if (afterBoss == false) {//ボスの前ならば
            if (sankaku.activeSelf) {//sankakuがアクティブならば
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {//Returnキー=Enterキー
                    sankaku.SetActive(false);
                    daialogBox.SetActive(false);
                    if (serifuNum == 4) {//最後のセリフの番号
                        Hanten();
                        transform.DOMoveX(1490, 1.0f);//画面外へ
                        GameManager.instance.playSE(escapeSE);
                        marichaAnimator.Play("RightIdle");
                        lastBoss.GetComponent<Enemy_Bullet_Boss>();
                        enemy_Bullet_Boss.battleStart = true;//ボス動き出す
                        maricha.GetComponent<Player>();
                        player.canControl = true;//プレイヤー操作可能に
                        vanishWall.SetActive(false);//左の壁を消す。
                        Invoke("Hanten", 1.5f);//1.5秒後画面外でれいみゅを反転させる。
                        return;
                    }//最後のセリフならこの下はいかない
                    serifuNum++;
                    if (serifuNum == 3) {//3セリフ目の前にラスボスが降ってくる。
                        lastBoss.SetActive(true);
                        Invoke("NextMessege", 2.0f);//ラスボスが着地してから、メッセージ表示
                    } else {
                        NextMessege();
                    }
                }
            }
        }


        if (afterBoss == true) {//ボス後
            if (sankaku.activeSelf) {//sankakuがアクティブならば
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {//Returnキー=Enterキー
                    sankaku.SetActive(false);
                    daialogBox.SetActive(false);
                    if (ending_serifuNum == 8) {//8回目のセリフの後にダイコンを降らせる。
                        //Invoke("DaiconActive(i)", daicontaime);//Invokeに引数は使えない。参考https://kan-kikuchi.hatenablog.com/entry/DelayMethod
                        daicons.SetActive(true);
                    }
                    if (ending_serifuNum == 15) {
                        animator.Play("RightSmile");
                        marichaAnimator.Play("RightSmile");
                    }
                    if (ending_serifuNum != 8 || (ending_serifuNum == 8 && daicons.activeSelf == true)) {
                        ending_serifuNum++;
                        NextMessege();
                    }
                }
            }
        }
    }


    void CanNextDaialog() {
        sankaku.SetActive(true);
    }
    void NextMessege() {
        if (afterBoss == false) {//ボス前
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
            GameManager.instance.playSE(audios[serifuNum]);
            Invoke("CanNextDaialog", yoin[serifuNum]);
            if (serifuNum == 3) {//3セリフ目、ラスボスが降ってきた後
                marichaAnimator.Play("RightDown");//まりちゃをダウンモーションにする。
            }
        }

        if(afterBoss == true) {//ボス後
            daialogeText.text = ending_serifu[ending_serifuNum];//テキスト変更　参考https://freesworder.net/unity-text-change/
            if (ending_washa[ending_serifuNum] == "れいみゅ") {
                textBox.sprite = reimyuImageBox;  //画像切り替え　参考https://futabazemi.net/unity/photo_change_collider/
                marichaName.SetActive(false);
                reimyuName.SetActive(true);
                niyunName.SetActive(false);
            } else if(ending_washa[ending_serifuNum] == "まりちゃ") {
                textBox.sprite = marichaImageBox; //オブジェクト切り替え　参考https://dodagon.com/unity/array1
                marichaName.SetActive(true);
                reimyuName.SetActive(false);
                niyunName.SetActive(false);
            } else {
                textBox.sprite = niyunImageBox; //オブジェクト切り替え　参考https://dodagon.com/unity/array1
                marichaName.SetActive(false);
                reimyuName.SetActive(false);
                niyunName.SetActive(true);
            }

            daialogBox.SetActive(true);
            GameManager.instance.playSE(ending_audios[ending_serifuNum]);
            Invoke("CanNextDaialog", ending_yoin[ending_serifuNum]);
            if(ending_serifuNum == 1) {//セリフ１の後
                transform.DOPath(
                     path       : new Vector3[] {new Vector3(1440, -30.2f, 0), new Vector3(1440, -10.2f, 0), new Vector3(1420, -10.2f, 4) }, //移動するポイント
                     duration   : 2.5f); //移動時間 //参考https://kan-kikuchi.hatenablog.com/entry/DOTween_Path
                audioSource.Play();//れいみゅについてるオーディオソースでBGMならす。

            }
            if(ending_serifuNum == 14) {
                animator.Play("RightEat");
                marichaAnimator.Play("RightEat");
            }
            if(ending_serifuNum == 15) {
                daicon01.SetActive(false);
                daicon02.SetActive(false);
            }
            if(ending_serifuNum == 16) {
                audioSource.Stop();
                animator.Play("RightPoison");
                marichaAnimator.Play("RightPoison");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (afterBoss == false) {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("Idlenaki")) {//泣いてるアニメーションの時しか反応しない。
                animator.Play("RightSmile");
                GameManager.instance.RandomizeSfx(ouenVoice1, ouenVoice2, ouenVoice3, ouenVoice4, ouenVoice5);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (afterBoss == false) {
            Invoke("NakuAnim", 5.0f);//離れてから5.0秒後に泣き出す。
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        animator.Play("RightSmile");
    }

    private void NakuAnim() {
        animator.Play("Idlenaki");
    }

    private void Hanten() {
        transform.Rotate(new Vector3(0, 180, 0));//れいみゅを反転させる。
    }

    public void EndingStart() {
        player.canControl = false;//プレイヤーを行動不可にする。
        fade.StartFadeOut();
        animator.Play("RightIdle");
        marichaAnimator.Play("RightIdle");
        Invoke("AfterBoss", 1.5f);
    }

    public void AfterBoss() {
        movingPlatformRight.SetActive(false);//左側の足場を消す。
        maricha.transform.position = new Vector3(1395, -10.2f, 0);
        camera2.SetActive(true);//カメラ2はfalseからtrueに切り替えないと、ゆっくりカメラ1からカメラ2に切り替わってしまう。
        camera1.SetActive(false);//カメラを変更。まりちゃ追従のままだとカメラ位置が高すぎるため。//参考https://nekojara.city/unity-cinemachine-change-target
        afterBoss = true;
        Invoke("AfterBoss2", 2.0f);
        boxCollider2.isTrigger = false;//れいみゅのボックスコライダー2dのisTriggerを外す。大根が滑ってれいみゅにめり込むため。
    }
    
    public void AfterBoss2() {
        fade.StartFadeIn();
        textBox.sprite = marichaImageBox; //オブジェクト切り替え　参考https://dodagon.com/unity/array1
        marichaName.SetActive(true);
        reimyuName.SetActive(false);
        daialogBox.SetActive(true);
        daialogeText.text = ending_serifu[ending_serifuNum];
        GameManager.instance.playSE(ending_audios[ending_serifuNum]);
        Invoke("CanNextDaialog", ending_yoin[ending_serifuNum]);
        marichaAnimator.Play("RightSmile");
        animator.Play("RightSmile");
    }
}

//エンディング用の音楽を鳴らす。
//8(ゆゆっ)のあとに野菜が振ってくる。
//一時的にセリフキャンバスを非アクティブに
//ダイコンを8つくらい、空中に置く
//ループで時間差で上から落とす。startで落下音

//14(むーしゃむーしゃ)でRightEatを再生
//15(ちちち、ちあわ)の前にダイコンを2つDestroyする。
//15でRightSmileを再生

//16(ゆげえええええええきゃらいいい)の前に
//音楽を停止
//RightPoisonを再生

//18で終了、ダイアログボックスを消す。おわりの文字をうえから降らす。
//けっかはっぴょう
//すこあにゆっくり残機×1000
//500点でどうばっじ、1000点でぎんばっじ、2000てんできんばっじ、3000てんでぷらちなばっじ
//それにおうじたこうかおんとまりちゃのせりふ
//点数いれる。
//ぷらちなばっじのばあいはさいごにえらいっとひょうじ。
//たいとるへボタンをひょうじ

//かならずafterbossをオフ、れいみゅとダイアログ関係のセットアクティブをオフにすること！