using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BulletController : MonoBehaviour{

    public float bulletSpeed;
    public Enemy_Bullet_Boss boss;

    // Start is called before the first frame update
    void Start(){
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
