using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour{

    private Animator anim = null;// アニメーターコンポーネントを入れる変数
    
    // Start is called before the first frame update
    void Start(){
        anim = GetComponent<Animator>();//アニメーターコンポーネントを取得、変数animに格納
        Debug.Log("ok1");
    }

    // Update is called once per frame
    void Update(){
        float horizontalKey = Input.GetAxis("Horizontal");//左右方向のインプットを取得

        if (horizontalKey > 0)//右方向の入力があった場合
        {
            Debug.Log("ok2");
            anim.SetBool("right", true);
            anim.SetBool("left", false);
            //参考動画では画像を反転させて左右への移動を処理
            //transform.localScale = new Vector3(1,1,1);
            //anim.SetBool("run", true);
        }
        else if (horizontalKey < 0)//左方向の入力があった場合
        {
            anim.SetBool("right", false);
            anim.SetBool("left", true);
            //transform.localScale = new Vector3(-1,1,1);
            //anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("right", false);
            anim.SetBool("left", false);
        }
    }
}
