using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//ボスのクラス
//MovingPlatFormをすり抜けさせるため、BossレイヤーとMovePlatFormレイヤーを作成
//MovingPlatFormのリジッドボディのPlatFormEffecterのコライダーマスクでBossレイヤーを無効化した。
//プロジェクト設定の物理、2d物理のマトリックスからチェックを外すのはなぜかうまく起動せず。参考https://unity-guide.moon-bear.com/layer-physics/


public class Enemy_Bullet_Boss : MonoBehaviour {
    #region//インスペクターで設定する
    [Header("加算スコア")] public int myScore;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;

    [Header("攻撃オブジェクト")] public GameObject attackObj;
    [Header("攻撃間隔")] public float interval;
    public int bossLife;
    private string playerTag = "Player";
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
        if (currentState.IsName("run")) {
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
                    transform.localScale = new Vector3(-20, 20, 1);//前二つはオブジェクトの大きさ
                } else {
                    transform.localScale = new Vector3(20, 20, 1);
                }
                rb.velocity = new Vector2(xVector * speed, -gravity);
            } else {
                rb.Sleep();
            }
        } else {
            //anim.Play("hit");
            //一瞬ボスのコライダーを消した方がよい？
            //一回踏むと跳ね返ってこなくなる
            //Invoke("Run", 1.0f);
        }
        if (bossLife > 0) {
            anim.Play("run");
        } else {
            rb.velocity = new Vector2(0, -gravity);
            isDead = true;
            col.enabled = false;//BoxCollider2Dを無効にする。
            Destroy(gameObject, 3f);
            //transform.Rotate(new Vector3(0, 0, 5)); 
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

    public void Run() {
        anim.Play("run");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.tag == playerTag){
            anim.Play("hit");
            --bossLife;
            ++speed;
        }
    }
}
