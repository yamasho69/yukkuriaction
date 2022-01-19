using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//レッスン49で作成
public class Score : MonoBehaviour{

    private Text scoreText = null;
    private int oldScore = 0;

    // Start is called before the first frame update
    void Start(){
        scoreText = GetComponent<Text>();
        if(GameManager.instance != null) {
            scoreText.text = "Score " + GameManager.instance.score;
        } else {
            Debug.Log("ゲームマネージャー置き忘れ");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update(){
        if(oldScore != GameManager.instance.score) {
            scoreText.text = "Score " + GameManager.instance.score;
            oldScore = GameManager.instance.score;
        }
    }
}
