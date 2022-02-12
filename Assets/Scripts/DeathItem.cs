using UnityEngine;
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
    [SerializeField] AudioClip clip4;
    [SerializeField] AudioClip clip5;
    [SerializeField] AudioClip morunmorunSE;

    //HeadCheckにPlayerタグをつけないと、下からアイテムをとったとき、アイテムが消えずに残ってしまう＝このクラスは実行されない

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (PlayerCheck.isOn) {
            //if (GameManager.instance != null) {
            if (!GameManager.instance.isGameOver) {//ゲームオーバー時にはダウンボイスを鳴らさない。
                GameManager.instance.RandomizeSfx(clip1, clip2, clip3, clip4, clip5);//ランダムでボイスを鳴らす。インスペクターのSfxsourceにはGameManagerをアタッチ。
                GameManager.instance.playSE(morunmorunSE);
            }   
                Destroy(this.gameObject);
            //}
        }
    }
}

