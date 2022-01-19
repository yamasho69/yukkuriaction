using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Heart : MonoBehaviour{
    private Text heartText = null;
    private int oldHeart = 0;

    // Start is called before the first frame update
    void Start() {
        heartText = GetComponent<Text>();
        if (GameManager.instance != null) {
            heartText.text = "× " + GameManager.instance.heart;
        } else {
            Debug.Log("ゲームマネージャー置き忘れ");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update() {
        if (oldHeart != GameManager.instance.heart) {
            heartText.text = "× " + GameManager.instance.heart;
            oldHeart = GameManager.instance.heart;
        }
    }
}