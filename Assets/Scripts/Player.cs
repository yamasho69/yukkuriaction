using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour{

    //インスペクターで変数を定義できるようにする。
    public float speed;//速度
    public float jumpSpeed;//ジャンプ速度
    public float jumpHeight; //ジャンプする高さ。40で追加。
    public float jumpLimitTime;//ジャンプ制限時間。40で追加。
    public float grovity;//重力
    public GroundCheck ground; //レッスン38　設置判定で追加
    public GroundCheck head;//頭をぶつけた時の判定。40で追加


    private Animator anim = null;// アニメーターコンポーネントを入れる変数
    private Rigidbody2D rb = null;//リジッドボディ２Ｄを入れる。
    private bool isGround = false;//38で追加。
    private bool isHead = false;//40で追加。
    private bool isJump = false;//40で追加。
    private float jumpPos = 0.0f;//40で追加。ジャンプした高さを記録。
    private float jumpTime = 0.0f;//40で追加。滞空時間をはかる。
    
    // Start is called before the first frame update
    void Start(){
        anim = GetComponent<Animator>();//アニメーターコンポーネントを取得、変数animに格納
        rb = GetComponent<Rigidbody2D>();//リジッドボディ２Ⅾを取得、変数rbに格納
    }

    // FixedUpdate1 物理演算の前にしたい処理を書く。可能なら重い処理は他へ。瞬間的な処理は相性が悪い。レッスン39
    void FixedUpdate(){

        //設置判定を得る
        isGround = ground.IsGround();
        isHead = head.IsGround();

        float horizontalKey = Input.GetAxis("Horizontal");//左右方向のインプットを取得
        float verticalKey = Input.GetAxis("Vertical");//上下方向のインプットを取得、レッスン40で追加
        float xSpeed = 0.0f; //Speed変数を入れる変数
        float ySpeed = -grovity; //レッスン40で追加。何もしていないときは重力がそのままかかる

        if (isGround) {
            if(verticalKey > 0) {
                ySpeed = jumpSpeed;//設置時に上方向のキー入力があったらジャンプ
                jumpPos = transform.position.y; //ジャンプした高さを記録
                isJump = true;
                jumpTime = 0.0f;//滞空時間をリセット。
                anim.SetBool("jump", true);
            } else {
                isJump = false;
                anim.SetBool("jump", false);
            }
        }else if(isJump){
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

            if(pushUpKey && canHeight && canTime && !isHead) {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;//上昇している間に進んだゲーム内時間を加算
            } else {
                isJump = false;
                jumpTime = 0.0f;//ジャンプタイムをリセット
                anim.SetBool("jump", false);
            }
        }

        if (horizontalKey > 0)//右方向の入力があった場合
        {
            anim.SetBool("right", true);
            anim.SetBool("left", false);
            //参考動画では画像を反転させて左右への移動を処理
            //transform.localScale = new Vector3(1,1,1);
            //anim.SetBool("run", true);
            xSpeed = speed;//右なら正の方向のSpeed変数
        }
        else if (horizontalKey < 0)//左方向の入力があった場合
        {
            anim.SetBool("right", false);
            anim.SetBool("left", true);
            //transform.localScale = new Vector3(-1,1,1);
            //anim.SetBool("run", true);
            xSpeed = -speed;//右なら負の方向のSpeed変数
        }
        else
        {
            anim.SetBool("right", false);
            anim.SetBool("left", false);
            xSpeed = 0.0f;
        }
        //velocityをスクリプトで上書きし、物理法則を無視させる
        //参考：https://www.youtube.com/watch?v=klTg9hl_clU
        rb.velocity = new Vector2(xSpeed, ySpeed);//レッスン40で第二引数をySpeedに変更
    }
}
