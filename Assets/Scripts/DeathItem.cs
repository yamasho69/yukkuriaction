using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeathItem : MonoBehaviour {
    [Header("プレイヤーの判定")] public PlayerTriggerCheck PlayerCheck;
    [Header("デスアイテムボイス")] public AudioClip [] deathitemvoices;
    [SerializeField] AudioClip clip1;
    [SerializeField] AudioClip clip2;
    [SerializeField] AudioClip clip3;
    [SerializeField] AudioClip clip4;
    [SerializeField] AudioClip clip5;
    [SerializeField] AudioClip morunmorunSE;
    public bool isWatar;//これをオンにしていると水。破壊されないためにオンにする。
    private List<BoxCollider2D> cols;　//複数のボックスコライダーをリストで取得。https://baba-s.hatenablog.com/entry/2014/08/29/092959
    private bool touched;//水に触った後かどうか。

    //HeadCheckにPlayerタグをつけないと、下からアイテムをとったとき、アイテムが消えずに残ってしまう＝このクラスは実行されない

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (PlayerCheck.isOn) {
            if (!GameManager.instance.isGameOver) {//ゲームオーバー時にはダウンボイスを鳴らさない。
                GameManager.instance.RandomizeSfx(deathitemvoices);//ランダムでボイスを鳴らす。インスペクターのSfxsourceにはGameManagerをアタッチ。
                GameManager.instance.playSE(morunmorunSE);
                if (isWatar) {//これが水の場合
                    PlayerCheck.isOn = false;
                    WatarTouch();
                    //col = this.GetComponent<BoxCollider2D>();←これではだめ。一つのコンポーネントしか取得しない。
                    //col.enabled = false;//プレイヤーが触ったらすぐコライダーをオフに←同上
                    Invoke("WatarTouch", 1.5f);//1.5秒後にコライダーを復活させる。

                }
            }
            if (!isWatar) {
                Destroy(this.gameObject);
            }             
        }
    }

    void WatarTouch() {
        var cols = new List<BoxCollider2D>();　//ボックスコライダーをリストを作成
        GetComponents<BoxCollider2D>(cols);　//ゲットコンポーネントして、リストに入れる。
        if (!touched) {//水に触っていない状態で水に触る
            touched = true;//水に触り済になる
            foreach (var col in cols) {
                col.enabled = false;//全てのボックスコライダーを非アクティブにする。
            }
        } else {//水に触っている状態ならば
            foreach (var col in cols) {
                col.enabled = true;//全てのボックスコライダーをアクティブにする。
            }
            touched = false;//水に触っていない状態にする。
        }
    }
}

