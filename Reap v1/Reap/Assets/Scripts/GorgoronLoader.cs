using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GorgoronLoader : MonoBehaviour {
	// Use this for initialization
    bool Started = false;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        double progress = Application.GetStreamProgressForLevel("Gorgoron7");
        if(progress ==1 && !Started){
            Application.LoadLevelAsync("Gorgoron7");
            Started = true;
        }
	}
}