using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class AmmoMedRequestListener : MonoBehaviour {
	private static int COOLDOWN = 1;

	public static AmmoMedRequestListener self;
	private KeyCode spawnMed = KeyCode.F12;
	private KeyCode spawnAmmo = KeyCode.F11;

	private IList<AmmoMedSpawnerScript> scripts;

	private Boolean isWaiting;

	// Use this for initialization
	void Start () {
		self = this;
		scripts = new List<AmmoMedSpawnerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isWaiting) {
			return;
		}

		if (Input.GetKey (spawnMed)) {
			spawnMedPack();
			StartCoroutine(Wait ());

		}
		
		if (Input.GetKey(spawnAmmo)) {
			spawnAmmoPack();
			StartCoroutine(Wait ());
		}
	}

	IEnumerator Wait() {
		isWaiting = true;
		yield return new WaitForSeconds(COOLDOWN);
		isWaiting = false;
	}

	public void register(AmmoMedSpawnerScript script) {
		scripts.Add (script);
		Debug.Log (scripts.Count);
	} 

	private void spawnMedPack() {
		System.Random rnd = new System.Random();
		int r = rnd.Next(scripts.Count);
		Debug.Log (r);
		scripts[r].spawnMed();
	}

	private void spawnAmmoPack() {
		System.Random rnd = new System.Random();
		int r = rnd.Next(scripts.Count);
		scripts[r].spawnAmmo();
	}
}
