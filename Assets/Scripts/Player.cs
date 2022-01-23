﻿using UnityEngine;
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
    [Header("JumpVoice")] public AudioClip jumpVoice;
    [Header("RightDownVoice")] public AudioClip rightDownVoice;
    [Header("LeftDownVoice")] public AudioClip leftDownVoice;
    [Header("FallVoice")] public AudioClip fallVoice;

    #endregion

    #region
    private Animator anim = null;// アニメーターコンポーネントを入れる変数
    private Rigidbody2D rb = null;//リジッドボディ２Ｄを入れる。
    private CapsuleCollider2D capcol = null;//レッスン45で追加。カプセルコライダー2Dを入れる。
    private SpriteRenderer sr = null;// スプライトを入れる変数。レッスン50で追加。
    private bool isGround = false;//38で追加。
    private bool isHead = false;//40で追加。
    private bool isJump = false;//40で追加。
    private bool isOtherJump = false;//45で追加。敵を踏んだ時の跳ね返り。
    private bool isRight = false; //43で追加。
    private bool isLeft = false;//43で追加。
    private bool isDown = false;//44で追加。
    private bool isContinue = false; //50で追加。
    private bool nonDownAnim = false;//51
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

        if (!isDown && !GameManager.instance.isGameOver) {//ダウン中ではない。レッスン44で追加。
            //設置判定を得る
            isGround = ground.IsGround();
            isHead = head.IsGround();

            //各種座標軸の速度を求める
            float ySpeed = GetYSpeed();
            float xSpeed = GetXSpeed();

            SetAnimation();

            //velocityをスクリプトで上書きし、物理法則を無視させる
            //参考：https://www.youtube.com/watch?v=klTg9hl_clU
            rb.velocity = new Vector2(xSpeed, ySpeed);//レッスン40で第二引数をySpeedに変更
        } else {
            rb.velocity = new Vector2(0, -grovity);//レッスン44で追加。ダウン中は重力のみ影響
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
        }else if (isGround) {
            if (verticalKey > 0) {
                ySpeed = jumpSpeed;//設置時に上方向のキー入力があったらジャンプ
                jumpPos = transform.position.y; //ジャンプした高さを記録
                isJump = true;
                jumpTime = 0.0f;//滞空時間をリセット。
            } else {
                isJump = false;
            }
        }else if (isJump) {
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
        if (isJump||isOtherJump) {
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }
        return ySpeed;

    }

    private float GetXSpeed() {
        float horizontalKey = Input.GetAxis("Horizontal");//左右方向のインプットを取得
        float xSpeed = 0.0f; //Speed変数を入れる変数

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

        //踏みつけ判定になる高さ
        float stepOnHeight = (capcolSizeY * (stepOnRate / 100f));//動画コメントを参考に変更
        //踏みつけ判定のワールド座標
        float judgePos = transform.position.y - ((capcolSizeY / 2f)-capcolOffsetY) + stepOnHeight;//動画コメントを参考に変更


        if (collision.collider.tag == enemyTag) {
            //レッスン45で追加。敵と衝突した位置を検知。当たったらまずい場所に当たっていたらミス
            foreach (ContactPoint2D p in collision.contacts) {
                if (p.point.y <judgePos){
                    //もう一度跳ねる
                    ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();//スクリプトObjectCollisionから跳ねる高さを取得
                    GameManager.instance.playSE(jumpVoice);
                    if (o != null) {
                        otherJumpHeight = o.boundHeight;//踏んづけたものから跳ねる高さを取得する。
                        o.playerStepOn = true;
                        jumpPos = transform.position.y;
                        isOtherJump = true;
                        isJump = false;
                        jumpTime = 0.0f;
                    }


                }else{
                    ReceiveDamage(true);
                    break;//ダウンがあったらループを抜ける。
                }
            }
        }
    }

    public bool IsContinueWaiting() {
        if (GameManager.instance.isGameOver) {
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
        if (isDown) {
            return;
        } else 
        {
            if (downAnim) {
                AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
                if (isLeft || currentState.IsName("LeftIdle") || currentState.IsName("LeftLanding")) {
                    anim.Play("LeftDown");
                    GameManager.instance.playSE(leftDownVoice);
                } else {
                    anim.Play("RightDown");
                    GameManager.instance.playSE(rightDownVoice);
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
        if(collision.tag == deadAreaTag) {
            GameManager.instance.playSE(fallVoice);
            ReceiveDamage(false);
        }else if(collision.tag == hitAreaTag){
            ReceiveDamage(true);
        }
    }
}
