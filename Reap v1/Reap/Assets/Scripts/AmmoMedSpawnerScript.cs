using UnityEngine;
using System.Collections;

public class AmmoMedSpawnerScript : MonoBehaviour {
	public GameObject medPackObject;
	public GameObject ammoPackObject;
    public GameObject huffObject;
	public Transform startingLocation;

	// Use this for initialization
	void Start () {
		AmmoMedRequestListener.self.register(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void spawnMed() {
		GameObject o = Instantiate(medPackObject) as GameObject;
		o.transform.position = startingLocation.position;
	}

	public void spawnAmmo() {
		GameObject o = Instantiate(ammoPackObject) as GameObject;
		o.transform.position = startingLocation.position;
	}

    public void spawnHuff() {
        GameObject o = Instantiate(huffObject) as GameObject;
        o.transform.position = startingLocation.position;
    }
}
