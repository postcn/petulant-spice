using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public GameObject body;
    public GameObject fireDeathReplacement;
    public int health;

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
        } else if (this.health <= 0) {
            kill(Constants.DEATH_REASONS.Fighting);
        }
	}

    protected virtual void kill(Constants.DEATH_REASONS reason) {
       
        if (this.GetType().Equals(typeof(Hero_Management))) {
            DaleManagement.self.Fired();
            Hero_Management.maximizeCamera();
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
