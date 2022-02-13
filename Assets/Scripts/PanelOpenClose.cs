using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PanelOpenClose : MonoBehaviour{

    public GameObject openpanel;
    public GameObject closepanel;
    public bool credit;
    [Header("CreditVoice1")] public AudioClip creditVoice1;
    [Header("CreditVoice2")] public AudioClip creditVoice2;

    public void OnClick() {
        if (closepanel != null) {
            closepanel.SetActive(false);
        }
        if (openpanel != null) {
            openpanel.SetActive(true);
        }
        if (credit) {
            GameManager.instance.RandomizeSfx(creditVoice1, creditVoice2);
        }
    }
}
