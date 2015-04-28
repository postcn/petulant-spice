using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public GameObject body;
    protected int health;

    public Character() {}

    protected Character(int health, GameObject body) {
        this.health = health;
        this.body = body;
    }

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	    if (this.transform.position.y < Constants.MAP_FLOOR) {
            kill();
        }
	}

    protected virtual void kill() {
        Debug.Log("Should be killed");

        if (this is Hero_Management) {
            Debug.Log("Hero ded. Game Over.");
            //TODO: Implement Game over.
        }
        //TODO: Monster -> heal the hero's bloodlust by amount.
        else if (this is Enemy) {
            //TODO: Enemies have different amounts?
            Hero_Management.self.decrementBloodlust();
        }
        Destroy(body);
    }
}
