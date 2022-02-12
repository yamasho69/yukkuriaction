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
    [Header("この点数を超えると残機アップ")] public int zankiUpScore;//自分で足した。
    [Header("デフォルトの残機")] public int defaultHeartNum;
    [HideInInspector] public bool isGameOver = false;
    [HideInInspector] public bool isStageClear = false;
    [Header("GameOverSE")] public AudioClip gameOverSE;
    [Header("OneUpSE")] public AudioClip OneUpSE;

    private AudioSource audioSource = null;
    public AudioSource sfxSource;

    private void Awake() {
        if(instance == null) {
            instance = this;
            zankiUpScore = 100;//自分で設定。100点越えたら残機アップ
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
        
    }

    private void Start() {
        //audioSource = GetComponent<AudioSource>();
    }

    public void AddHeartNum() {
        if(heart < 99) {
            ++heart;
            playSE(OneUpSE);
        }
    }

    public void SubHeartNum() {
        if(heart > 0) {
            --heart;
        } else {
            //GameOverになったとき
            isGameOver = true;
            playSE(gameOverSE);
        }
    }

    public void RetryGame() {
        isGameOver = false;
        heart = defaultHeartNum;
        score = 0;
        //stageNum = 1;
        continueNum = 0;
        zankiUpScore = 100;
    }

    public void playSE(AudioClip clip) {
        if(audioSource != null) {
            audioSource.PlayOneShot(clip);
        } else {
            Debug.Log("オーディオソースが設定されていません。");
        }
    }

    //ランダムでボイスを鳴らす、参考http://negi-lab.blog.jp/archives/RandomizeSfx.html
    public void RandomizeSfx(params AudioClip[] clips) {
        var randomIndex = UnityEngine.Random.Range(0, clips.Length);
        sfxSource.PlayOneShot(clips[randomIndex]);
    }
}

