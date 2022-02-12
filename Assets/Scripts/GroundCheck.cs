using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//参考：https://www.youtube.com/watch?v=-0uaqD0m514

public class GroundCheck : MonoBehaviour{

    [Header("エフェクトがついた床を判定")] public bool checkPlatformGround;
    public string groundTag = "Ground";
    public string moveFloorTag = "MoveFloor";
    public string platformTag = "GroundPlatform";
    public string fallFloorTag = "FallFloor";//55
    public bool isGround = false;
    public bool isGroundEnter, isGroundStay, isGroundExit;
    
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
            isGroundEnter = true;
        } else if(checkPlatformGround && (collision.tag == platformTag|| collision.tag == moveFloorTag || collision.tag == fallFloorTag)) {//レッスン54で追加。下から抜けられる床の判定
            isGroundEnter = true;
        } 
    }

    //判定内から出たときに呼ばれる
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.tag == groundTag) {
            isGroundExit = true;
        } else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag)) {//レッスン54で追加。下から抜けられる床の判定
            isGroundExit = true;
        }
    }

    //判定内に居続ける限り呼ばれる(長すぎると呼ばれなくなる)
    private void OnTriggerStay2D(Collider2D collision) {
          if(collision.tag == groundTag || collision.tag == fallFloorTag) {
            isGroundStay = true;
        } else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag)) {//レッスン54で追加。下から抜けられる床の判定
            isGroundStay = true;
        }
    }
}