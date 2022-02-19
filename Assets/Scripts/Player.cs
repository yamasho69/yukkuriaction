using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour{

    //インスペクターで変数を定義できるようにする。
    #region
    [Header("移動速度")] public float speed;//速度
    [Header("ジャンプ速度")] public float jumpSpeed;//ジャンプ速度
    [Header("ジャンプの高さ")] public float jumpHeight; //ジャンプする高さ。40で追加。
    [Header("ジャンプ制限時間")] public float jumpLimitTime;//ジャンプ制限時間。40で追加。
    [Header("踏みつけ判定の高さの割合")] public float stepOnRate;//レッスン45で追加。
    [Header("重力")] public float grovity;//重力
    [Header("設置判定")] public GroundCheck ground; //レッスン38　設置判定で追加
    [Header("頭をぶつけた時の判定")] public GroundCheck head;//頭をぶつけた時の判定。40で追加
    [Header("ダッシュアニメーションカーブ")] public AnimationCurve dashCurve;//レッスン41で追加。アニメーションカーブ
    [Header("ジャンプアニメーションカーブ")] public AnimationCurve jumpCurve;//同上
    [Header("JumpVoice1")] public AudioClip jumpVoice1;
    [Header("JumpVoice2")] public AudioClip jumpVoice2;
    [Header("JumpVoice3")] public AudioClip jumpVoice3;
    [Header("enemyDownVoice1")] public AudioClip enemyDownVoice1;
    [Header("enemyDownVoice2")] public AudioClip enemyDownVoice2;
    [Header("enemyDownVoice3")] public AudioClip enemyDownVoice3;
    [Header("enemyDownVoice4")] public AudioClip enemyDownVoice4;
    [Header("enemyDownVoice5")] public AudioClip enemyDownVoice5;
    [Header("enemyDownVoice6")] public AudioClip enemyDownVoice6;
    [Header("trapDownVoice1")] public AudioClip trapDownVoice1;
    [Header("trapDownVoice2")] public AudioClip trapDownVoice2;
    [Header("trapDownVoice3")] public AudioClip trapDownVoice3;
    [Header("trapDownVoice4")] public AudioClip trapDownVoice4;
    [Header("trapDownVoice5")] public AudioClip trapDownVoice5;
    [Header("trapDownVoice6")] public AudioClip trapDownVoice6;
    [Header("heatDownVoice1")] public AudioClip heatDownVoice1;
    [Header("heatDownVoice2")] public AudioClip heatDownVoice2;
    [Header("heatDownVoice3")] public AudioClip heatDownVoice3;
    [Header("heatDownVoice4")] public AudioClip heatDownVoice4;
    [Header("heatDownVoice5")] public AudioClip heatDownVoice5;
    [Header("heatDownVoice6")] public AudioClip heatDownVoice6;
    [Header("FallVoice1")] public AudioClip fallVoice1;
    [Header("FallVoice2")] public AudioClip fallVoice2;
    [Header("FallVoice3")] public AudioClip fallVoice3;
    [Header("FallVoice4")] public AudioClip fallVoice4;
    [Header("FallVoice5")] public AudioClip fallVoice5;
    [Header("morunmorun")] public AudioClip morunmorunSE;
    [Header("enemyDeath")] public AudioClip enemyDeathSE;
    [Header("trapSE")] public AudioClip trapSE;
    [Header("heatSE")] public AudioClip heatSE;
    [Header("WatarSE")] public AudioClip watarSE;
    [Header("pauseButton")] public GameObject pauseButton;
    public GameObject reimyu;
    public bool canControl = true;
    #endregion

    #region
    private Animator anim = null;// アニメーターコンポーネントを入れる変数
    private Rigidbody2D rb = null;//リジッドボディ２Ｄを入れる。
    public CapsuleCollider2D capcol = null;//レッスン45で追加。カプセルコライダー2Dを入れる。
    private SpriteRenderer sr = null;// スプライトを入れる変数。レッスン50で追加。
    private MoveObject moveObj = null;//54
    private bool isGround = false;//38で追加。
    private bool isHead = false;//40で追加。
    public bool isJump = false;//40で追加。
    private bool isOtherJump = false;//45で追加。敵を踏んだ時の跳ね返り。
    private bool isRight = false; //43で追加。
    private bool isLeft = false;//43で追加。
    private bool isDown = false;//44で追加。
    private bool isContinue = false; //50で追加。
    private bool nonDownAnim = false;//51
    private bool isClearMotion = false; //56で追加。
    private float continueTime = 0.0f;//50で追加。
    private float blinkTime = 0.0f;//50で追加。
    private float jumpPos = 0.0f;//40で追加。ジャンプした高さを記録。
    private float otherJumpHeight = 0.0f; //45で追加。
    private float jumpTime = 0.0f;//40で追加。滞空時間をはかる。
    private float dashTime = 0.0f;//41
    private float beforeKey = 0.0f;//41
    private string enemyTag = "Enemy"; //44で追加。敵判別。
    private string deadAreaTag = "DeadArea";//51
    private string hitAreaTag = "HitArea";//51
    private string moveFloorTag = "MoveFloor";//54
    private string fallFloorTag = "FallFloor"; //55
    private string poisonTag = "Poison"; //唐辛子につけるタグ
    private string watarTag = "Watar"; //水につけるタグ
    private string heatAreaTag = "HeatArea"; //鉄板につけるタグ
    private string lastTag = "LastContinuePoint"; //最後のチェックポイントにつけるタグ
    #endregion

    // Start is called before the first frame update
    void Start(){
        anim = GetComponent<Animator>();//アニメーターコンポーネントを取得、変数animに格納
        rb = GetComponent<Rigidbody2D>();//リジッドボディ２Ⅾを取得、変数rbに格納
        capcol = GetComponent<CapsuleCollider2D>();//レッスン45で追加。
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (isContinue) {
            //明滅　ついている時に戻る
            if (blinkTime > 0.2f) {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            //明滅　消えているとき
            else if (blinkTime > 0.1f) {
                sr.enabled = false;
            }
            //明滅　ついているとき
            else {
                sr.enabled = true;
            }
            //1秒たったら明滅終わり
            if (continueTime > 1.0f) {
                isContinue = false;
                blinkTime = 0.0f;
                continueTime = 0.0f;
                sr.enabled = true;
            } else {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }
    }

    // FixedUpdate1 物理演算の前にしたい処理を書く。可能なら重い処理は他へ。瞬間的な処理は相性が悪い。レッスン39
    void FixedUpdate() {
        if (isClearMotion) {//ここは動画、ブログと違う。クリアモーションがオンになると動かないようにする。
            rb.velocity = new Vector2(0, -grovity);
        }
        else if (!isDown && !GameManager.instance.isGameOver) {//ダウン中ではない。レッスン44で追加。ここは死なない限り有効
            //設置判定を得る
            isGround = ground.IsGround();
            isHead = head.IsGround();

            if(isHead == true && isGround == true) {//自分で追加。頭の衝突判定と接地判定が同時に有効になる場合はジャンプできなくなる。
                head.isGround = false;//そのため、同時に有効になったら、頭の衝突判定を外す。インスペクター上でチェックを入れても、すぐに外れる。
            }


            //各種座標軸の速度を求める
            float ySpeed = GetYSpeed();
            float xSpeed = GetXSpeed();

            SetAnimation();

            //54
            Vector2 addVelocity = Vector2.zero;
            if(moveObj != null) {
                addVelocity = moveObj.GetVelocity();
            }
            //velocityをスクリプトで上書きし、物理法則を無視させる
            //参考：https://www.youtube.com/watch?v=klTg9hl_clU
            rb.velocity = new Vector2(xSpeed, ySpeed)+addVelocity;//レッスン40で第二引数をySpeedに変更
        } else {
            rb.velocity = new Vector2(0, -grovity);//レッスン44で追加。ダウン中は重力のみ影響
        }
        if (!isClearMotion && GameManager.instance.isStageClear) {//ここはブログとは違う。上に死なない限り有効な部分があるので、そこから分岐させてしまうとクリアしても有効にならない。
            isClearMotion = true;
            anim.Play("StageClear");
            //rb.velocity = new Vector2(0, -grovity);をここに書いても、死なない限り有効の部分で打ち消されるため、isClearMotionが有効ならば動かないif関数を一番上にした。
        }
    }

    private void SetAnimation() {
        anim.SetBool("jump", isJump||isOtherJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("left", isLeft);
        anim.SetBool("right", isRight);
    }


    private float GetYSpeed() {
        float verticalKey = Input.GetAxis("Vertical");//上下方向のインプットを取得、レッスン40で追加
        float ySpeed = -grovity; //レッスン40で追加。何もしていないときは重力がそのままかかる

        if (canControl) {//自分で追加。canControlがfalseだと入力を受け付けない。
            //何かを踏んだ際のジャンプ レッスン45で追加。
            if (isOtherJump) {
                //現在の高さが飛べる高さより下か
                bool canHeight = jumpPos + otherJumpHeight > transform.position.y;
                //ジャンプ時間が長くなりすぎてないか
                bool canTime = jumpLimitTime > jumpTime;

                if (canHeight && canTime && !isHead) {
                    ySpeed = jumpSpeed;
                    jumpTime += Time.deltaTime;
                } else {
                    isOtherJump = false;
                    jumpTime = 0.0f;
                }
            } else if (isGround) {
                if (verticalKey > 0) {
                    ySpeed = jumpSpeed;//設置時に上方向のキー入力があったらジャンプ
                    jumpPos = transform.position.y; //ジャンプした高さを記録
                    isJump = true;
                    jumpTime = 0.0f;//滞空時間をリセット。
                } else {
                    isJump = false;
                }
            } else if (isJump) {
                //ジャンプした位置+ジャンプできる高さ=飛べる高さ
                //つまり上方向キーが押されている間かつ自分が飛べる高さにいるとき
                //if (verticalKey > 0 && jumpPos + jumpHeight > transform.position.y) {


                //ifの条件式が長くなりすぎるのでレッスン40で変数にして整理した。
                //上方向のキーを押しているか
                bool pushUpKey = verticalKey > 0;
                //現在の高さが飛べる高さより下か
                bool canHeight = jumpPos + jumpHeight > transform.position.y;
                //ジャンプ時間が長くなりすぎていないか
                bool canTime = jumpLimitTime > jumpTime;

                if (pushUpKey && canHeight && canTime && !isHead) {
                    ySpeed = jumpSpeed;
                    jumpTime += Time.deltaTime;//上昇している間に進んだゲーム内時間を加算
                } else {
                    isJump = false;
                    jumpTime = 0.0f;//ジャンプタイムをリセット
                }
            }
        }
        if (isJump||isOtherJump) {
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }
        return ySpeed;

    }

    private float GetXSpeed() {

        float horizontalKey = Input.GetAxis("Horizontal");//左右方向のインプットを取得
        float xSpeed = 0.0f; //Speed変数を入れる変数
        if (canControl) {//自分で追加。canControlがfalseになると、入力を受け付けない。
            if (horizontalKey > 0)//右方向の入力があった場合
            {
                isRight = true;
                isLeft = false;
                dashTime += Time.deltaTime;//41
                                           //参考動画では画像を反転させて左右への移動を処理
                                           //transform.localScale = new Vector3(1,1,1);
                xSpeed = speed;//右なら正の方向のSpeed変数
            } else if (horizontalKey < 0)//左方向の入力があった場合
              {
                isRight = false;
                isLeft = true;
                dashTime += Time.deltaTime;//41
                                           //transform.localScale = new Vector3(-1,1,1);
                xSpeed = -speed;//右なら負の方向のSpeed変数
            } else {
                isLeft = false;
                isRight = false;
                dashTime = 0.0f;
                xSpeed = 0.0f;
            }

            //レッスン41で追加。前回のキー入力と方向が違うと加速を０にする。
            if ((horizontalKey > 0 && beforeKey < 0) || (horizontalKey < 0 && beforeKey > 0)) {
                dashTime = 0.0f;
            }
        }
        beforeKey = horizontalKey;

        //アニメーションカーブを速度に適用
        xSpeed *= dashCurve.Evaluate(dashTime);

        return xSpeed;
    }

    //レッスン44で追加。OnTrigerEnterと間違えないように
    private void OnCollisionEnter2D(Collision2D collision) {
        //踏みつけ判定になる高さ
        //float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));

        //踏みつけ判定のワールド座標
        //float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;



        float capcolSizeY = capcol.size.y * transform.localScale.y;//動画コメントを参考に追加。
        float capcolOffsetY = capcol.offset.y * transform.localScale.y;//動画コメントを参考に追加。

        //レッスン55でフラグ整理
        bool enemy = (collision.collider.tag == enemyTag);
        bool moveFloor = (collision.collider.tag == moveFloorTag);
        bool fallFloor = (collision.collider.tag == fallFloorTag);
        bool poison = (collision.collider.tag == poisonTag);//自分で作成。唐辛子につけるタグ。

        if (enemy && !isDown) {//ここはenemyのみにする。isDownを入れないと二重にダメージをくらう

            //踏みつけ判定になる高さ
            float stepOnHeight = (capcolSizeY * (stepOnRate / 100f));//動画コメントを参考に変更
                                                                     //踏みつけ判定のワールド座標
            float judgePos = transform.position.y - ((capcolSizeY / 2f) - capcolOffsetY) + stepOnHeight;//動画コメントを参考に変更

            //レッスン45で追加。敵と衝突した位置を検知。当たったらまずい場所に当たっていたらミス
            foreach (ContactPoint2D p in collision.contacts) {
                if (p.point.y < judgePos) {
                    //もう一度跳ねる
                    ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();//スクリプトObjectCollisionから跳ねる高さを取得
                    GameManager.instance.playSE(enemyDeathSE);//敵を踏みつけた時の音
                    GameManager.instance.RandomizeSfx(jumpVoice1, jumpVoice2, jumpVoice3);
                    if (o != null) {
                        if (enemy) {
                            otherJumpHeight = o.boundHeight;//踏んづけたものから跳ねる高さを取得する。
                            o.playerStepOn = true;
                            jumpPos = transform.position.y;
                            isOtherJump = true;
                            isJump = false;
                            jumpTime = 0.0f;
                        }
                    }
                } else {
                    if (enemy) {
                        ReceiveDamage(true);
                        if (!GameManager.instance.isGameOver) {//ゲームオーバー時にはダウンボイスを鳴らさない。
                            GameManager.instance.RandomizeSfx(enemyDownVoice1, enemyDownVoice2, enemyDownVoice3, enemyDownVoice4,enemyDownVoice5,enemyDownVoice6);//ReceiveDamage内で声を出さないようにしたのでここに入れる。
                            GameManager.instance.playSE(morunmorunSE);
                            Untagged();
                        }
                        break;//ダウンがあったらループを抜ける。
                    }
                }
            }
        } else if (moveFloor) {//54
            float stepOnHeight = (capcolSizeY * (stepOnRate / 100f));
            float judgePos = transform.position.y - ((capcolSizeY / 2f) - capcolOffsetY) + stepOnHeight;
            foreach (ContactPoint2D p in collision.contacts) {
                if (p.point.y < judgePos) {
                    moveObj = collision.gameObject.GetComponent<MoveObject>();
                }
            }
        }
        if (fallFloor) {//レッスン55で追加。ブログ、動画のやり方とは違う。
            ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
            o.playerStepOn = true;
        }
    }
    //54
    private void OnCollisionExit2D(Collision2D collision) {
        if(collision.collider.tag == moveFloorTag) {
            moveObj = null;
        }
    }

    public bool IsContinueWaiting() {
        if (GameManager.instance.isGameOver) {
            capcol.enabled = false;//ゲームオーバーの時はプレイヤーのカプセルコライダーをオフにして、画面外に落とす
            pauseButton.SetActive(false);//ゲームオーバーの時はポーズボタンを非表示にする。
            return false;
        } else {
            return IsDownAnimEnd() || nonDownAnim;
        }
    }

    //ダウンアニメーションが完了しているかどうか。レッスン50で追加。
    private bool IsDownAnimEnd() {
        if(isDown && anim != null) {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("RightDown") || currentState.IsName("LeftDown")) {
                if(currentState.normalizedTime >= 1) {
                    return true;
                }
            }
        }
        return false;
    }

    //レッスン50で追加。
    public void ContinuePlayer() {
        this.tag = "Player";
        head.tag = "Player";
        isJump = false;
        isOtherJump = false;
        isRight = false;
        isLeft = false;
        isContinue = true;
        anim.Play("RightIdle");
        isDown = false;
        isContinue = true;
        nonDownAnim = false;
    }

    //51で追加。
    private void ReceiveDamage(bool downAnim) {
        if (isDown || GameManager.instance.isStageClear) {
            return;
        } else 
        {
            if (downAnim) {
                AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
                if (isLeft || currentState.IsName("LeftIdle") || currentState.IsName("LeftLanding") || currentState.IsName("LeftFall")) {
                    anim.Play("LeftDown");
                    //GameManager.instance.playSE(leftDownVoice); 
                } else {
                    anim.Play("RightDown");
                    //GameManager.instance.playSE(rightDownVoice);
                }
            } else {
                nonDownAnim = true;
            }
        }
        isDown = true;
        GameManager.instance.SubHeartNum();
    }

    //51で追加。
    private void OnTriggerEnter2D(Collider2D collision) {
        if ((collision.tag == watarTag || collision.tag == poisonTag) && !isDown) {//ダウン中に唐辛子をとっても反応しない様に!isDownをつける。
            ReceiveDamage(true);
            if (collision.tag == watarTag) {
                GameManager.instance.playSE(watarSE);//水のタグなら鳴らす効果音
            }
        } else if (collision.tag == hitAreaTag && !isDown) {//!isDownを付けないとトゲの上にいる限り反応する
            ReceiveDamage(true);
            GameManager.instance.playSE(trapSE);
            if (!GameManager.instance.isGameOver) {//ゲームオーバー時にはダウンボイスを鳴らさない。
                GameManager.instance.RandomizeSfx(trapDownVoice1, trapDownVoice2, trapDownVoice3, trapDownVoice4, trapDownVoice5, trapDownVoice6);
                GameManager.instance.playSE(morunmorunSE);
                Untagged();
            }
        } else if (collision.tag == heatAreaTag && !isDown) {//!isDownを付けないと鉄板の上にいる限り反応する
            ReceiveDamage(true);
            GameManager.instance.playSE(heatSE);
            if (!GameManager.instance.isGameOver) {//ゲームオーバー時にはダウンボイスを鳴らさない。
                GameManager.instance.RandomizeSfx(heatDownVoice1, heatDownVoice2, heatDownVoice3, heatDownVoice4, heatDownVoice5, heatDownVoice6);
                GameManager.instance.playSE(morunmorunSE);
                //Untagged();
            }
        } else if (collision.tag == deadAreaTag && !isDown) {//ダウン状態で穴に落ちた状態のときに反応しないように追加。
            ReceiveDamage(false);
            if (!GameManager.instance.isGameOver) {//ゲームオーバー時にはダウンボイスを鳴らさない。
                GameManager.instance.RandomizeSfx(fallVoice1, fallVoice2, fallVoice3, fallVoice4, fallVoice5);
            }
        }

        if(collision.tag == lastTag && reimyu != null) {
            canControl = false;//入力をできないようにする。
            isRight = false;
            reimyu.SetActive(true);
        }
    }
    


    //自分で作成。トゲ(トラップ)を触った跡にPlayerタグを外し、アイテムを取らないようにする。
    //http://negi-lab.blog.jp/archives/12259004.html
    //コンテニュー時はPlayerタグをつけなおす。毒アイテムは近くに並べて置かないこと。
    //毒アイテムはDeathItemクラスで別に音を出しているため、タグを外してしまうと、音が鳴らなくなり、毒アイテムもDestroyされなくなる。
    //今後の課題とする。
    public void Untagged() {
        this.tag = "Untagged";
        head.tag = "Untagged";
    }
}
