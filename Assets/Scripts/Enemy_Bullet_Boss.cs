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
    [Header("通常移動速度")] public float baseSpeed;
    [Header("最高移動速度")] public float maxSpeed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;

    [Header("攻撃オブジェクト")] public GameObject attackObj;
    [Header("攻撃間隔")] public float interval;
    public int bossLife;
    private string playerTag = "Player";
    private SpriteRenderer sp = null;
    public float mutekitime;
    
    
    #endregion

    private float continueTime = 0.0f;//50で追加。
    private float blinkTime = 0.0f;//50で追加。


    #region//プライベート変数
    private float speed = 0.0f;
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    public bool rightTleftF = false;
    private bool isDead = false;
    public bool isDamage = false;
    private float timer;
    #endregion

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        speed = baseSpeed;

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

        if (isDamage) {
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
                //無敵時間がたったら明滅終わり
            if (continueTime > mutekitime) {
                isDamage = false;    
                speed = baseSpeed;
                maxSpeed += 10.0f;
                blinkTime = 0.0f;
                continueTime = 0.0f;
                sr.enabled = true;
                mutekitime += 0.5f;
            } else {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }
    }

    void FixedUpdate() {
        if (!oc.playerStepOn) {
            if (sr.isVisible || nonVisibleAct) {
                if (checkCollision.isOn) {
                    rightTleftF = !rightTleftF;
                    Attack();
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
            if (!isDamage && bossLife >0) {
                isDamage = true;
                oc.playerStepOn = false;
                //効果音
                anim.Play("hit");
                --bossLife;
                speed = maxSpeed;
                //Invoke("Damage", 5.0f);
            }
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
}
