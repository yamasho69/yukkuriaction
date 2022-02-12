using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerTriggerCheck : MonoBehaviour{

    [HideInInspector] public bool isOn = false;

    private string playerTag = "Player";


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == playerTag) {
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == playerTag) {
            isOn = false;
        }
    }
}
