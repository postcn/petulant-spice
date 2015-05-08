using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public GameObject body;
    public GameObject fireDeathReplacement;
    protected int health;

    public Character() {}

    protected Character(int health, GameObject body) {
        this.health = health;
        this.body = body;
    }

    public int getHealth() {
        return this.health;
    }

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	    if (this.transform.position.y <= Constants.MAP_FLOOR) {
            kill(Constants.DEATH_REASONS.Fire);
        }
	}

    protected virtual void kill(Constants.DEATH_REASONS reason) {
        Debug.Log("Should be killed");

        if (this is Hero_Management) {
            Debug.Log("Hero ded. Game Over.");
            //TODO: Implement Game over.
        }
        //TODO: Monster -> heal the hero's bloodlust by amount.
        else if (this is Enemy) {
            //TODO: Enemies have different amounts?
            if (Hero_Management.self != null) {
                Hero_Management.self.decrementBloodlust();
                Hero_Management.self.addSamples(this.getSampleCount());
            }

        }
        if(reason == Constants.DEATH_REASONS.Fire) {
            Instantiate(fireDeathReplacement);
            fireDeathReplacement.transform.position = body.transform.position;
        }
        Destroy(body);
    }

    protected virtual int getSampleCount() {
        return 0;
    }
}
