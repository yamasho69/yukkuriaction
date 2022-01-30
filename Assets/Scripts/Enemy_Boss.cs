using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


//レッスン59で作成
public class Enemy_Boss : MonoBehaviour{
    
    private enum BossState {
        StartEnsyutu,
        Battle,
        ClearEnsyutu
    }

    private BossState nowState = BossState.StartEnsyutu;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        switch (nowState) {
            case BossState.StartEnsyutu:
                StartEnsyutu();
                break;
            case BossState.Battle:
                break;
            case BossState.ClearEnsyutu:
                break;
        }
    }
    private void StartEnsyutu() {
        
    }

}
