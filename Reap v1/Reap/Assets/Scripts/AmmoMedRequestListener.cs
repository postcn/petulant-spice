using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class AmmoMedRequestListener : MonoBehaviour {

    public AudioClip[] timeDenials;
    public AudioClip[] moneyDenials;

    public AudioClip[] medSuccess;
    public AudioClip[] ammoSuccess;

	private const int COOLDOWN = 60;

    private const int MEDPACK_COST = 150;
    private const int AMMO_COST = 100;

	public static AmmoMedRequestListener self;
	private KeyCode spawnMed = KeyCode.F12;
	private KeyCode spawnAmmo = KeyCode.F11;

	private IList<AmmoMedSpawnerScript> scripts;

	private Boolean isWaiting;
    private Boolean playingAudio;

	// Use this for initialization
	void Start () {
		self = this;
		scripts = new List<AmmoMedSpawnerScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if( DaleManagement.self != null && DaleManagement.self.isAFK()) {
            return;
        } 

        if (playingAudio) {
            return;
        }

        if (isWaiting && (Input.GetKey (spawnMed) || Input.GetKey(spawnAmmo))) {
            StartCoroutine(PlayRandomAudio(timeDenials));
            return;
		} else if (isWaiting) {
            return;
        }

		if (Input.GetKey (spawnMed) && canBuyMed()) {
			spawnMedPack();
			StartCoroutine(Wait());

		}
		
		if (Input.GetKey(spawnAmmo) && canBuyAmmo()) {
			spawnAmmoPack();
			StartCoroutine(Wait());
		}
	}

    IEnumerator PlayRandomAudio(AudioClip[] clips) {
        playingAudio = true;
        System.Random rnd = new System.Random();
        int index = rnd.Next(clips.Length);
        AudioSource.PlayClipAtPoint(clips[index], Camera.main.transform.position);
        yield return new WaitForSeconds(clips[index].length);
        playingAudio = false;
    }

	IEnumerator Wait() {
		isWaiting = true;
		yield return new WaitForSeconds(COOLDOWN);
		isWaiting = false;
	}


	public void register(AmmoMedSpawnerScript script) {
		scripts.Add(script);
	}

    private Boolean canBuyMed() {
        if (Hero_Management.self != null) {
            bool val = Hero_Management.self.getSamplesCollected() >= MEDPACK_COST;
            if (!val) {
                StartCoroutine(PlayRandomAudio(moneyDenials));
            }
            return val;
        } else {
            return false;
        }
    }

    private Boolean canBuyAmmo() {
        if (Hero_Management.self != null) {
            bool val = Hero_Management.self.getSamplesCollected() >= AMMO_COST;
            if (!val) {
                StartCoroutine(PlayRandomAudio(moneyDenials));
            }
            return val;
        } else {
            return false;
        }
    }

	private void spawnMedPack() {
		System.Random rnd = new System.Random();
		int r = rnd.Next(scripts.Count);
		scripts[r].spawnMed();
        Hero_Management.self.removeSamples(MEDPACK_COST);
        StartCoroutine(PlayRandomAudio(medSuccess));
	}

	private void spawnAmmoPack() {
		System.Random rnd = new System.Random();
		int r = rnd.Next(scripts.Count);
		scripts[r].spawnAmmo();
        Hero_Management.self.removeSamples(AMMO_COST);
        StartCoroutine(PlayRandomAudio(ammoSuccess));
	}
}
