using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public GameObject body;
    private int health;

    public Character() {}

    protected Character(int health, GameObject body) {
        this.health = health;
        this.body = body;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected void kill() {
        Debug.Log("Should be killed");
        Destroy(body);
    }
}
