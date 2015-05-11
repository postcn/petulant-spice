using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void GoToGorgoron(){

        Application.LoadLevel ("GorgoronLoader"); 
    }

    public void GoToCave(){
        Application.LoadLevelAsync ("CaveLoader"); 
    }

    public void GoToHome(){
        Application.LoadLevel ("TitleScreen");
    }
}
