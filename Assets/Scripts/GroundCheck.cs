﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//参考：https://www.youtube.com/watch?v=-0uaqD0m514

public class GroundCheck : MonoBehaviour{

    private string groundTag = "Ground";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;
    
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    //物理判定の更新の度に呼ぶ必要がある
    //PlayerScriptにisGroundの値を返す。
    public bool IsGround() {
        if(isGroundEnter || isGroundStay) {
            isGround = true;
        }
        else if (isGroundExit) {
            isGround = false;
        }
        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    //判定内に入ったときに呼ばれる
    private void OnTriggerEnter2D(Collider2D collision) {//判定内に入った２D コライダーの
        if (collision.tag == groundTag) {//タグがGroundならば
            //Debug.Log("判定");
            isGroundEnter = true;
        }
    }

    //判定内から出たときに呼ばれる
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.tag == groundTag) {
            isGroundExit = true;
        }
    }

    //判定内に居続ける限り呼ばれる(長すぎると呼ばれなくなる)
    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == groundTag) {
            isGroundStay = false;
        }
    }
}