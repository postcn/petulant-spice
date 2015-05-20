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
        Hero_Management.removeSamples(Hero_Management.getSamplesCollected());
        Application.LoadLevel ("TitleScreen");
    }

    public void Quit() {
        Application.Quit();
    }
}
