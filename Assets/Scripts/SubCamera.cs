using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SubCamera : MonoBehaviour{

    public Transform tr;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start(){
        tr = GetComponent<Transform>();
        tr = mainCamera.transform;
    }

    // Update is called once per frame
    void Update(){
        
    }
}
