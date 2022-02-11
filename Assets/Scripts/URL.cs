using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//参考にしたサイト　https://getabakoclub.com/2019/11/06/unity%E3%81%A7url%E3%82%92%E9%96%8B%E3%81%8F/

public class URL : MonoBehaviour {

    public string url;//自分で付け加えた

    public void onClick() {
        Application.OpenURL(url);//参考サイトの通りだとURL直打ちなので、インスペクターからURL設定ができるように変更
    }
}