using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//自分で作成　参考https://shiro-changelife.com/shootiing-bullet/

public class BulletController : MonoBehaviour{

    public float bulletSpeed;
    public Enemy_Bullet_Boss boss;
    [Header("アタック")] public AudioClip attack;

    // Start is called before the first frame update
    void Start(){
        GameManager.instance.playSE(attack);//オブジェクト生成されたときになる
    }

    // Update is called once per frame
    void Update() {
        Vector3 bulletPos = transform.position;
        if (boss.rightTleftF) {
            bulletPos.x += -bulletSpeed * Time.deltaTime;
        } else { bulletPos.x += bulletSpeed * Time.deltaTime;}
        transform.position = bulletPos;
    }
}
