using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//レッスン50で作成
public class StageCtrl : MonoBehaviour{

    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;
    [Header("コンテニュー位置")] public GameObject[] continuePoint;

    private Player p;
    
    // Start is called before the first frame update
    void Start(){
        if (playerObj != null && continuePoint != null && continuePoint.Length > 0) 
        {
            playerObj.transform.position = continuePoint[0].transform.position;

            p = playerObj.GetComponent<Player>();
            if(p == null) {
                Debug.Log("プレイヤーじゃないものがアタッチされているよ。");
            }
        } 
        else 
        {
            Debug.Log("設定が足りていないよ！");
        }
    }

    // Update is called once per frame
    void Update(){
        if(p!= null && p.IsContinueWaiting()) {
            if(continuePoint.Length > GameManager.instance.continueNum) {
                playerObj.transform.position = continuePoint[GameManager.instance.continueNum].transform.position;
                p.ContinuePlayer();
            } else 
            {
                Debug.Log("コンテニューポイントの設定が足りてないよ！");
            }
        }
    }
}
