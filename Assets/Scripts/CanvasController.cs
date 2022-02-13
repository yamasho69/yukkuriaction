using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

//このページを参考に作成。[Unity] DoTweenでuGUIにバウンドさせてみる

public class CanvasController : MonoBehaviour{

    [Header("UeVoice")] public AudioClip UeVoice;
    [Header("ShitaVoice")] public AudioClip ShitaVoice;
    public Image imageUe;
    public Image imageShita;
    public GameObject button1;
    public GameObject button2;
    public GameObject copytext;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void CanvasOn() {
        gameObject.SetActive(true);
        Invoke("TitleUeDown", 1.5f);
    }

    public void TitleUeDown() {
        imageUe.transform.DOLocalMoveY(150f, 1.5f).SetEase(Ease.OutBounce);
        GameManager.instance.playSE(UeVoice);
        Invoke("TitleShitaSlide", 1.5f);
    }

    public void TitleShitaSlide() {
        imageShita.transform.DOLocalMoveX(0f, 1.5f).SetEase(Ease.OutBounce);
        GameManager.instance.playSE(ShitaVoice);
        Invoke("ButtonOn", 1.0f);
    }

    public void ButtonOn() {
        button1.SetActive(true);
        button2.SetActive(true);
        copytext.SetActive(true);
    }
}
