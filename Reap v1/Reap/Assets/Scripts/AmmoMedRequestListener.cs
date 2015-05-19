using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class AmmoMedRequestListener : MonoBehaviour {

    public AudioClip[] timeDenials;
    public AudioClip[] moneyDenials;

    public AudioClip[] medSuccess;
    public AudioClip[] ammoSuccess;
    public AudioClip[] huffSuccess;

	private const int COOLDOWN = 15;

    private const int MEDPACK_COST = 75;
    private const int AMMO_COST = 50;
    private const int HUFF_COST = 125;

	public static AmmoMedRequestListener self;
	private KeyCode spawnMed = KeyCode.F12;
	private KeyCode spawnAmmo = KeyCode.F11;
    private KeyCode spawnHuff = KeyCode.F10;

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

        if (isWaiting && (Input.GetKey(spawnMed) || Input.GetKey(spawnAmmo) || Input.GetButton("LeftShoulder") || Input.GetButton("RightShoulder"))) {
            StartCoroutine(PlayRandomAudio(timeDenials));
            return;
		} else if (isWaiting) {
            return;
        }

		if ((Input.GetKey(spawnMed) || Input.GetButton("LeftShoulder")) && canBuyMed()) {
			spawnMedPack();
			StartCoroutine(Wait());

		}
		
        if ((Input.GetKey(spawnAmmo) || Input.GetButton("RightShoulder")) && canBuyAmmo()) {
			spawnAmmoPack();
			StartCoroutine(Wait());
		}

		//TODO: Swap out to correct button.
        if ((Input.GetKey(spawnHuff) || Input.GetButton("RightShoulder")) && canBuyHuff()) {
			spawnHuffCan(Input.GetKey(spawnHuff));
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
        if (Hero_Management.mousePlayer != null || Hero_Management.controllerPlayer != null) {
            bool val = Hero_Management.getSamplesCollected() >= MEDPACK_COST;
            if (!val) {
                StartCoroutine(PlayRandomAudio(moneyDenials));
            }
            return val;
        } else {
            return false;
        }
    }

    private Boolean canBuyAmmo() {
		if (Hero_Management.mousePlayer != null || Hero_Management.controllerPlayer != null) {
            bool val = Hero_Management.getSamplesCollected() >= AMMO_COST;
            if (!val) {
                StartCoroutine(PlayRandomAudio(moneyDenials));
            }
            return val;
        } else {
            return false;
        }
    }

    private Boolean canBuyHuff() {
		if (Hero_Management.mousePlayer != null || Hero_Management.controllerPlayer != null) {
            bool val = Hero_Management.getSamplesCollected() >= HUFF_COST;
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
        Hero_Management.removeSamples(MEDPACK_COST);
        StartCoroutine(PlayRandomAudio(medSuccess));
	}

	private void spawnAmmoPack() {
		System.Random rnd = new System.Random();
		int r = rnd.Next(scripts.Count);
		scripts[r].spawnAmmo();
        Hero_Management.removeSamples(AMMO_COST);
        StartCoroutine(PlayRandomAudio(ammoSuccess));
	}

    private void spawnHuffCan(bool mousePlayer) {
        AmmoMedSpawnerScript closest = scripts[0];
        float closestDist = distanceToHero(closest.startingLocation, mousePlayer ? Hero_Management.mousePlayer : Hero_Management.controllerPlayer);
        foreach (AmmoMedSpawnerScript script in scripts) {
			float dist = distanceToHero(script.startingLocation, mousePlayer ? Hero_Management.mousePlayer : Hero_Management.controllerPlayer);
            if (dist < closestDist) {
                closestDist = dist;
                closest = script;
            }
        }
        closest.spawnHuff();
        Hero_Management.removeSamples(HUFF_COST);
        StartCoroutine(PlayRandomAudio(huffSuccess));
    }

    private float distanceToHero(Transform position, Hero_Management triggeringHero) {
        return Mathf.Abs(Vector3.Distance(position.position, triggeringHero.gameObject.transform.position));
    }
}
