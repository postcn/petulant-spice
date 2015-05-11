using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GorgoronLoader : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        double progress = Application.GetStreamProgressForLevel("Gorgoron7");
        if(progress ==1){
            Application.LoadLevelAsync("Gorgoron7");
        }
	}
}
