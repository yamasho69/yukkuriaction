using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//トランジションさせるスクリプト
//参考https://qiita.com/Holtzakai/items/7c90a76adcac4b3aa7cd#fn2
//Imageへのアタッチが必要。

public class Transition : MonoBehaviour {
    [SerializeField]
    private Material _transitionIn;
    [Header("オチのセリフ")] public AudioClip ochiVoice;

    void Start() {
        StartCoroutine(BeginTransition());
    }

    IEnumerator BeginTransition() {
        yield return Animate(_transitionIn, 2);//第二引数がトランジションにかかる時間
    }

    /// <summary>
    /// time秒かけてトランジションを行う
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Animate(Material material, float time) {
        GetComponent<Image>().material = material;
        float current = 0;
        while (current < time / 2) {//timeの2分の1までしかループしないので0.5で止まる。
            material.SetFloat("_Alpha", current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        material.SetFloat("_Alpha", 0.5f);//0.5fでアルファ値？タイムをセット。
        GameManager.instance.playSE(ochiVoice);
        yield return new WaitForSeconds(5);//コルーチン処理5秒待つhttps://www.sejuku.net/blog/83712
        while (current < time) {//timeの2分の1までしかループしないので0.5で止まる。
            material.SetFloat("_Alpha", current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        material.SetFloat("_Alpha", 1);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("ResultScene");
    }
}