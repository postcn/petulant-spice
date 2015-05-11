using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CaveLoader : MonoBehaviour {
	// Use this for initialization
    bool Started = false;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        double progress = Application.GetStreamProgressForLevel("CoolCave");
        if(progress ==1 && !Started){
            Application.LoadLevelAsync("CoolCave");
            Started = true;
        }
	}
}
