﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingSceneScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        double progress = Application.GetStreamProgressForLevel("CoolCave");
        if(progress ==1){
            Application.LoadLevelAsync("CoolCave");
        }
	}
}