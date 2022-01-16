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
    #endregion

    #region
    private Animator anim = null;// アニメーターコンポーネントを入れる変数
    private Rigidbody2D rb = null;//リジッドボディ２Ｄを入れる。
    private CapsuleCollider2D capcol = null;//レッスン45で追加。カプセルコライダー2Dを入れる。
    private bool isGround = false;//38で追加。
    private bool isHead = false;//40で追加。
    private bool isJump = false;//40で追加。
    private bool isOtherJump = false;//45で追加。敵を踏んだ時の跳ね返り。
    private bool isRight = false; //43で追加。
    private bool isLeft = false;//43で追加。
    private bool isDown = false;//44で追加。
    private float jumpPos = 0.0f;//40で追加。ジャンプした高さを記録。
    private float otherJumpHeight = 0.0f; //45で追加。
    private float jumpTime = 0.0f;//40で追加。滞空時間をはかる。
    private float dashTime = 0.0f;//41
    private float beforeKey = 0.0f;//41
    private string enemyTag = "Enemy"; //44で追加。敵判別。
    #endregion

    // Start is called before the first frame update
    void Start(){
        anim = GetComponent<Animator>();//アニメーターコンポーネントを取得、変数animに格納
        rb = GetComponent<Rigidbody2D>();//リジッドボディ２Ⅾを取得、変数rbに格納
        capcol = GetComponent<CapsuleCollider2D>();//レッスン45で追加。
    }

    // FixedUpdate1 物理演算の前にしたい処理を書く。可能なら重い処理は他へ。瞬間的な処理は相性が悪い。レッスン39
    void FixedUpdate() {

        if (!isDown) {//ダウン中ではない。レッスン44で追加。
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
            Debug.Log("ダウン");
        }
    }

    private void SetAnimation() {
        anim.SetBool("jump", isJump||isOtherJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("left", isLeft);
        anim.SetBool("right", isRight);
        //anim.SetBool("Down", isDown);
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

                    if(o != null) {
                        otherJumpHeight = o.boundHeight;//踏んづけたものから跳ねる高さを取得する。
                        o.playerStepOn = true;//
                        jumpPos = transform.position.y;
                        isOtherJump = true;
                        isJump = false;
                        jumpTime = 0.0f;
                    }


                }else{
                    //ダウンする
                    //anim.Play("LeftDown");
                    Debug.Log("衝突");
                    isDown = true;
                    break;//ダウンがあったらループを抜ける。
                }
            }
        }
    }
}
