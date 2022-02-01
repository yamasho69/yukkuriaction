using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScoreItem : MonoBehaviour{

    [Header("加算するスコア")] public int myScore;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck PlayerCheck;
    [SerializeField] AudioClip clip1;
    [SerializeField] AudioClip clip2;
    [SerializeField] AudioClip clip3;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if (PlayerCheck.isOn) {
            if(GameManager.instance != null) {
                GameManager.instance.score += myScore;
                if (GameManager.instance.score  >= GameManager.instance.zankiUpScore) {//自分で追加。スコアが100ごとに残機プラス1
                    GameManager.instance.zankiUpScore += 100;
                    GameManager.instance.AddHeartNum();
                }
                GameManager.instance.RandomizeSfx(clip1,clip2,clip3);//ランダムでボイスを鳴らす。インスペクターのSfxsourceにはGameManagerをアタッチ。
                Destroy(this.gameObject);
            }
        }
    }
}
