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
    public int score;
    public int stageNum;
    public int continueNum;
    public int heart;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
        
    }
}
