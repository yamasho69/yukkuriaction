using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageNuｍ : MonoBehaviour{

    private Text stageText = null;
    private int oldStage = 0;

    // Start is called before the first frame update
    void Start() {
        stageText = GetComponent<Text>();
        if (GameManager.instance != null) {
            stageText.text = "Stage" + GameManager.instance.stageNum;
        } else {
            Debug.Log("ゲームマネージャー置き忘れ");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update() {
        if (oldStage != GameManager.instance.stageNum) {
            stageText.text = "Stage " + GameManager.instance.stageNum;
            oldStage = GameManager.instance.stageNum;
        }
    }
}
