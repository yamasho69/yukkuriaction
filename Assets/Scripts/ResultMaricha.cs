using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class ResultMaricha : MonoBehaviour{

    public GameObject kekkahappyo_moji;//結果発表の文字
    public AudioClip kekkahappyo; //結果発表のボイス
    public AudioClip scoreroll; //結果発表のボイス
    public GameObject score;
    public GameObject zanki;
    public GameObject konkainokekka;
    public AudioClip konkainokekkaVoice;
    
    //引数1
    public Sprite puratinaBadge;
    public Sprite goldBadge;
    public Sprite silverBadge;
    public Sprite bronzeBadge;
    public Sprite poop;

    public Image badgeImage;
    public GameObject badgeImageSprite;

    [Header("えらいっふきだし")] public GameObject Erai;
    [Header("タイトルへボタン")] public GameObject titleButton;
    [Header("えらいっボイス")] public AudioClip EraiVoice;

    //引数2
    [Header("プラチナバッジ級ボイス")] public AudioClip puratinaBadgeVoice;
    [Header("金バッジ級ボイス")] public AudioClip goldBadgeVoice;
    [Header("銀バッジ級ボイス")] public AudioClip silverBadgeVoice;
    [Header("銅バッジ級ボイス")] public AudioClip bronzeBadgeVoice;
    [Header("バッジなしボイス")] public AudioClip noBadgeVoice;

    //引数3
    [Header("プラチナバッジ級効果音")] public AudioClip puratinaBadgeSE;
    [Header("金バッジ級効果音")] public AudioClip goldBadgeSE;
    [Header("銀バッジ級効果音")] public AudioClip silverBadgeSE;
    [Header("銅バッジ級効果音")] public AudioClip bronzeBadgeSE;
    [Header("バッジなし効果音")] public AudioClip noBadgeSE;

    //引数4
    [Header("プラチナバッジ級文字")] public String puratinaString;
    [Header("金バッジ級文字")] public String goldString;
    [Header("銀バッジ級文字")] public String silverString;
    [Header("銅バッジ級文字")] public String bronzeString;
    [Header("バッジなし文字")] public String noString;

    [Header("プラチナバッジ級文字色")] public String puratinaStringColor;
    [Header("金バッジ級文字色")] public String goldStringColor;
    [Header("銀バッジ級文字色")] public String silverStringColor;
    [Header("銅バッジ級文字色")] public String bronzeStringColor;
    [Header("バッジなし文字色")] public String noStringColor;

    public Text badgeshurui;
    public GameObject badgeshuruiSprite;

    [Header("まりちゃアニメーター")] public Animator marichaanim;
    [Header("れいみゅアニメーター")] public Animator reimyuanim;
    [Header("プラチナバッジ級アニメーション")] public String puratinaAnim;
    [Header("金バッジ級アニメーション")] public String goldAnim;
    [Header("銀バッジ級アニメーション")] public String silverAnim;
    [Header("銅バッジ級アニメーション")] public String bronzeAnim;
    [Header("バッジなしアニメーション")] public String noAnim;
    //引数5

    [Header("プラチナバッジ級感想ボイス")] public AudioClip puratinaBadgeVoice2;
    [Header("金バッジ級感想ボイス")] public AudioClip goldBadgeVoice2;
    [Header("銀バッジ級感想ボイス")] public AudioClip silverBadgeVoice2;
    [Header("銅バッジ級感想ボイス")] public AudioClip bronzeBadgeVoice2;
    [Header("バッジなし感想ボイス")] public AudioClip noBadgeVoice2;

    public Text badgeKanso;
    public GameObject badgeKansoSprite;

    [Header("プラチナバッジ級感想")] public String puratinaKansoString;
    [Header("金バッジ級感想")] public String goldKansoString;
    [Header("銀バッジ級感想")] public String silverKansoString;
    [Header("銅バッジ級感想")] public String bronzeKansoString;
    [Header("バッジなし感想")] public String noKansoString;


    // Start is called before the first frame update
    void Start(){
        StartCoroutine(StartResult());
        reimyuanim.Play(silverAnim);
    }

    IEnumerator StartResult() {
        yield return new WaitForSeconds(2);//コルーチン処理2秒待つhttps://www.sejuku.net/blog/83712
        kekkahappyo_moji.SetActive(true);
        kekkahappyo_moji.transform.DOLocalMoveY(30f, 1.5f).SetEase(Ease.OutBounce);
        GameManager.instance.playSE(kekkahappyo);
        yield return new WaitForSeconds(2);
        score.SetActive(true);
        if (GameManager.instance.heart > 0) {
        zanki.SetActive(true);}
        while (true) {
            if (GameManager.instance.heart > 0) {
                --GameManager.instance.heart;
                GameManager.instance.score += 1000;
                GameManager.instance.playSE(scoreroll);
                //ピロンという音
                yield return new WaitForSeconds(0.8f);
                continue;
            }
            break;
        }
        zanki.SetActive(false);
        yield return new WaitForSeconds(2);
        GameManager.instance.playSE(konkainokekkaVoice);
        konkainokekka.SetActive(true);
        yield return new WaitForSeconds(2);
        //今回の評価声
        //セットアクティブ今回の評価
        if (GameManager.instance.score >= 4000) {
            Badge(puratinaBadge,puratinaBadgeVoice,puratinaString,puratinaStringColor);
            
        }else if (GameManager.instance.score >= 3000) {
            Badge(goldBadge,goldBadgeVoice,goldString,goldStringColor);
        } else if (GameManager.instance.score >= 2000) {
            Badge(silverBadge,silverBadgeVoice,silverString,silverStringColor);
        }else if (GameManager.instance.score >= 1000) {
            Badge(bronzeBadge,bronzeBadgeVoice,bronzeString,bronzeStringColor);
        } else {
            Badge(poop,noBadgeVoice,noString,noStringColor);
        }
        yield return new WaitForSeconds(2);

        if (GameManager.instance.score >= 4000) {
            BadgeSE(puratinaBadgeSE,puratinaAnim);

        } else if (GameManager.instance.score >= 3000) {
            BadgeSE(goldBadgeSE,goldAnim);
        } else if (GameManager.instance.score >= 2000) {
            BadgeSE(silverBadgeSE,silverAnim);
        } else if (GameManager.instance.score >= 1000) {
            BadgeSE(bronzeBadgeSE,bronzeAnim);
        } else {
            BadgeSE(noBadgeSE,noAnim);
        }

        yield return new WaitForSeconds(2);

        if (GameManager.instance.score >= 4000) {
            BadgeKanso(puratinaKansoString, puratinaBadgeVoice2);
            yield return new WaitForSeconds(1.5f);
        } else if (GameManager.instance.score >= 3000) {
            BadgeKanso(goldKansoString, goldBadgeVoice2);
        } else if (GameManager.instance.score >= 2000) {
            BadgeKanso(silverKansoString, silverBadgeVoice2);
        } else if (GameManager.instance.score >= 1000) {
            BadgeKanso(bronzeKansoString, bronzeBadgeVoice2);
        } else {
            BadgeKanso(noKansoString, noBadgeVoice2);
        }
        yield return new WaitForSeconds(3);
        if (GameManager.instance.score >= 4000) {
            Erai.SetActive(true);
            GameManager.instance.playSE(EraiVoice);
            yield return new WaitForSeconds(1);
        }
        titleButton.SetActive(true);
    }

    private void Badge(Sprite a,AudioClip b,String c,String d) {
        badgeImage.sprite = a; //画像を切り替える(引数1)
        badgeImage.enabled = true;//画像を表示
        badgeImageSprite.SetActive(true);
        GameManager.instance.playSE(b);//〇〇級
        badgeshurui.text = c;//〇〇級という文字

        string colorString = d; // 赤色の16進数文字列
        Color newColor;
        ColorUtility.TryParseHtmlString(colorString, out newColor);
        badgeshurui.color = newColor;
        badgeshuruiSprite.SetActive(true);
    }

    void BadgeSE(AudioClip a,String b) {
        GameManager.instance.playSE(a);
        marichaanim.Play(b);
        reimyuanim.Play(b);
    }

    void BadgeKanso(String a,AudioClip b) {
        badgeKanso.text = a;
        GameManager.instance.playSE(b);
        badgeKansoSprite.SetActive(true);
    }
}
