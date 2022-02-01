﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeathItem : MonoBehaviour {
    [Header("プレイヤーの判定")] public PlayerTriggerCheck PlayerCheck;
    [SerializeField] AudioClip clip1;
    [SerializeField] AudioClip clip2;
    [SerializeField] AudioClip clip3;

    //HeadCheckにPlayerタグをつけないと、下からアイテムをとったとき、アイテムが消えずに残ってしまう＝このクラスは実行されず、Playerクラスだけ実行される。

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (PlayerCheck.isOn) {
            //if (GameManager.instance != null) {
                GameManager.instance.RandomizeSfx(clip1, clip2, clip3);//ランダムでボイスを鳴らす。インスペクターのSfxsourceにはGameManagerをアタッチ。
                Destroy(this.gameObject);
            //}
        }
    }
}

