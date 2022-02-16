using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;


public class Enemy_Zako1 : MonoBehaviour {
    #region//インスペクターで設定する
    [Header("加算スコア")] public int myScore;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;
    #endregion

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool rightTleftF = false;
    private bool isDead = false;
    #endregion

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate() {
        if (!oc.playerStepOn) {
            if (sr.isVisible || nonVisibleAct) {
                transform.DOShakePosition(1f, 1f, 1, 2, false, true).SetRelative(true).SetLink(gameObject); ;
                //Dotweenでふわふわ動かす。.SetRelatiive(true)をつけることで、相対座標を指定できる。
                //https://zenn.dev/ohbashunsuke/books/20200924-dotween-complete/viewer/dotween-15
                //https://zenn.dev/ohbashunsuke/books/20200924-dotween-complete/viewer/dotween-19


            } else {
                rb.Sleep();
            }
        } else {
            if (!isDead) {
                anim.Play("dead");
                rb.velocity = new Vector2(0, -25);
                isDead = true;
                col.enabled = false;//BoxCollider2Dを無効にする。
                if(GameManager.instance != null) {
                    GameManager.instance.score += myScore;
                    if (GameManager.instance.score >= GameManager.instance.zankiUpScore) {//自分で追加。スコアが100ごとに残機プラス1
                        GameManager.instance.zankiUpScore += 100;
                        GameManager.instance.AddHeartNum();
                    }
                }
                 Destroy(gameObject, 3f);
            } else {
                transform.Rotate(new Vector3(0, 0, 0));
            }
        }
    }
}
