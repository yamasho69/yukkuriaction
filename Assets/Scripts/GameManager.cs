using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//レッスン48で作成
public class GameManager : MonoBehaviour{

    public static GameManager instance = null;
    
    [Header("スコア")]public int score;
    [Header("現在のステージ")] public int stageNum;
    [Header("現在の復帰位置")] public int continueNum;
    [Header("現在の残機")] public int heart;
    [Header("デフォルトの残機")] public int defaultHeartNum;
    [HideInInspector] public bool isGameOver;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
        
    }

    public void AddHeartNum() {
        if(heart < 99) {
            ++heart;
        }
    }

    public void SubHeartNum() {
        if(heart > 0) {
            --heart;
        } else {
            isGameOver = true;
        }
    }

    public void RetryGame() {
        isGameOver = false;
        heart = defaultHeartNum;
        score = 0;
        stageNum = 1;
        continueNum = 0;
    }
}
