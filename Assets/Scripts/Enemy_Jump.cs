using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

//その場でジャンプする敵

public class Enemy_Jump: MonoBehaviour {
    #region//インスペクターで設定する
    [Header("加算スコア")] public int myScore;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;
    [Header("ジャンプ力")] public float jumpPower;
    [Header("ジャンプ時間")] public float duration;
    [Header("ジャンプ間隔")] public float interval;
    #endregion

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    //private bool rightTleftF = false;
    private bool isDead = false;

    private Transform myTransform = null;
    private float timer;
    #endregion

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();
        // transformを取得、https://www.sejuku.net/blog/50983
        myTransform = GetComponent<Transform>();
    }

    void Update() {
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
        if (!currentState.IsName("dead")) {

            //通常の状態
            if (currentState.IsName("idle")) {
                if (timer > interval) {
                   col.enabled = true;//当たり判定を復活させる。
                   anim.SetTrigger("jump");
                   this.transform.DOJump(myTransform.position, jumpPower, numJumps: 1, duration).SetLink(gameObject);
                    //Dotweenで元々いた座標にジャンプ
                    //https://section31.jp/gamedevelopment/unity/dotween2/
                    //SetLink(gameObject)がないと、エラーがでる。超重要。
                    //https://kan-kikuchi.hatenablog.com/entry/DOTween_SetLink
                    timer = 0.0f;  
                } else {
                    timer += Time.deltaTime;
                }
            }
        }
    }

    void FixedUpdate() {
        if (!oc.playerStepOn) {
            if (sr.isVisible || nonVisibleAct) {
                if (checkCollision.isOn) {
                    //rightTleftF = !rightTleftF;
                    anim.SetTrigger("idle");
                }
                /*int xVector = -1;
                if (rightTleftF) {
                    xVector = 1;
                    transform.localScale = new Vector3(-10, 10, 1);
                } else {
                    transform.localScale = new Vector3(10, 10, 1);

                    rb.velocity = new Vector2(xVector * speed, -gravity); */
            } else {
                rb.Sleep();
            }
        } else {
            if (!isDead) {
                anim.Play("dead");
                rb.velocity = new Vector2(0, -25);
                isDead = true;
                col.enabled = false;//BoxCollider2Dを無効にする。
                if (GameManager.instance != null) {
                    GameManager.instance.score += myScore;
                    if (GameManager.instance.score >= GameManager.instance.zankiUpScore) {//自分で追加。スコアが100ごとに残機プラス1
                        GameManager.instance.zankiUpScore += 100;
                        GameManager.instance.AddHeartNum();
                    }
                }
                Destroy(gameObject, 0.3f);
            }
        }
    }

    //プレイヤーに触った瞬間、敵の当たり判定を消す。
    //上から乗ったときに、当たり判定があるままだとプレイヤーに乗っかっておかしくなる。
    private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == "Player"&& !oc.playerStepOn) {
                col.enabled = false;
                rb.gravityScale = 0.0f;
                Invoke("ColOne", 0.1f);//プイレイヤーと接触してすぐに当たり判定を復活させる。
                                       //当たり判定がないままだと、床を突き抜けて落ちてしまう。
            }
    }


    //プレイヤーと敵の当たり判定が重なると、重ならないようにじわじわずれるため、不自然さはなくなる。
    private void ColOne() {
        col.enabled = true;
        rb.gravityScale = 1.0f;
    }

}
