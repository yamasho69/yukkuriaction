using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

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

    //以下エンディングのセリフの配列
    [Header("誰が話しているか")] public string[] ending_washa;
    [Header("セリフ内容")] [TextArea(1, 3)] public string[] ending_serifu;
    [Header("ボイス")] public AudioClip[] ending_audios;
    [Header("いつから次のメッセージにいけるか。")] public float[] ending_yoin;
    [Header("セリフ番号")] public int ending_serifuNum = -1;


    // Start is called before the first frame update
    void Start() {
        textBox.GetComponent<Image>();
        animator = GetComponent<Animator>();
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


        if(afterBoss == true) {//ボス後
            if (sankaku.activeSelf) {//sankakuがアクティブならば
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {//Returnキー=Enterキー
                    sankaku.SetActive(false);
                    daialogBox.SetActive(false);
                    ending_serifuNum++;
                    NextMessege();
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
            unknownName.SetActive(false);
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
            //二ゆんの場合の分岐描く

            daialogBox.SetActive(true);
            GameManager.instance.playSE(ending_audios[ending_serifuNum]);
            Invoke("CanNextDaialog", ending_yoin[ending_serifuNum]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName("Idlenaki")) {//泣いてるアニメーションの時しか反応しない。
            animator.Play("RightSmile");
            GameManager.instance.RandomizeSfx(ouenVoice1, ouenVoice2, ouenVoice3, ouenVoice4, ouenVoice5);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Invoke("NakuAnim", 3.5f);//離れてから3.5秒後に泣き出す。
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
        Debug.Log("呼ばれた。");
        player.canControl = false;//プレイヤーを行動不可にする。
        fade.StartFadeOut();
        movingPlatformRight.SetActive(false);//右側の足場を消す。
        transform.DOMove(new Vector3(1485f, -10.2f, 0f), 0.1f);//https://qiita.com/broken55/items/df152c061da759ad1471
        maricha.transform.DOMove(new Vector3(1470f, -10.2f, 0f), 0.1f);
        animator.Play("RightIdle");
        marichaAnimator.Play("RightIdle");
        Invoke("AfterBoss", 3.0f);
    }

    public void AfterBoss() {
        fade.StartFadeIn();
        afterBoss = true;
        textBox.sprite = marichaImageBox; //オブジェクト切り替え　参考https://dodagon.com/unity/array1
        marichaName.SetActive(true);
        reimyuName.SetActive(false);
        daialogBox.SetActive(true);
        Invoke("CanNextDaialog", 2.0f);
    }
}

/*
やったのじぇ！
おねーしゃー！
いもーちょ。
おねーしゃはすごいんぢゃよ！
ゆ、まりちゃはさいっきょうなのじぇ！
わるいわるいおやしゃいしゃんはせいっさいしたのじぇ！
ありがちょー！おねーしゃはえいっゆんだねっ！
さ、まりちゃたちのゆっくちぷれいしゅにかえるのじぇ！
うん、ゆっくちりきゃいしちゃよ！
ゆゆ？
→やさい(大根)がいっぱい降ってくる。
おやしゃいしゃんがいっぱいじゃよ！
やっぱち、おやしゃいしゃんはかってにはえてくるんぢゃね！
いもーちょ、いっちょにむーしゃむーしゃするんだじぇ！
ちょうだね！　じゃあゆっくち、
いただきまちゅ！(カメラ上にパン)
むーしゃむーしゃ！ちちち、ちあわ…
ゆげえええええ、かりゃいいいい、こりぇどくはいっちぇる！
*/
