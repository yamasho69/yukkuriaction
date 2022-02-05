using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//レッスン46で作成


public class Enemy_Bullet: MonoBehaviour {
    #region//インスペクターで設定する
    [Header("加算スコア")] public int myScore;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;

    [Header("攻撃オブジェクト")] public GameObject attackObj;
    [Header("攻撃間隔")] public float interval;
    #endregion



    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool rightTleftF = false;
    private bool isDead = false;

    private float timer;
    #endregion

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();

        if (anim == null || attackObj == null) {
            Debug.Log("設定が足りません");
            Destroy(this.gameObject);
        } else {
            attackObj.SetActive(false);
        }
    }

    void Update() {
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);

        //通常の状態
        if (currentState.IsName("idle")) {
            if (timer > interval) {
                anim.SetTrigger("attack");
                timer = 0.0f;
            } else {
                timer += Time.deltaTime;
            }
        }
    }

    void FixedUpdate() {
        if (!oc.playerStepOn) {
            if (sr.isVisible || nonVisibleAct) {
                if (checkCollision.isOn) {
                    rightTleftF = !rightTleftF;
                }
                int xVector = -1;
                if (rightTleftF) {
                    xVector = 1;
                    transform.localScale = new Vector3(-10, 10, 1);
                } else {
                    transform.localScale = new Vector3(10, 10, 1);
                }
                rb.velocity = new Vector2(xVector * speed, -gravity);
            } else {
                rb.Sleep();
            }
        } else {
            if (!isDead) {
                anim.Play("dead");
                rb.velocity = new Vector2(0, -gravity);
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
            } //敵が回転しながら落ちる演出を変更(発射された弾も一緒に回ってしまうため)
        }
    }

    //参考ページ　https://dkrevel.com/makegame-beginner/make-2d-action-shot-enemy/
    public void Attack() {
        GameObject g = Instantiate(attackObj);
        g.transform.SetParent(transform);
        g.transform.position = attackObj.transform.position;
        g.transform.rotation = attackObj.transform.rotation;
        g.SetActive(true);
    }
}
