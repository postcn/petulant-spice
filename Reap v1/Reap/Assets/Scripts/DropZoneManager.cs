using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropZoneManager : MonoBehaviour {
    public const int SAMPLES_NEEDED = 500;
    public static DropZoneManager self;
    IList<DropZone> dropZones;
    private bool active = false;

    private const int MIN_DROP_WAIT = 60;
    private const int MAX_DROP_WAIT = 240;

    public AudioClip[] dropShipInbound;
    public AudioClip[] dropShipDelayed;
    public AudioClip[] dropShipArrived;

	// Use this for initialization
	void Start () {
        self = this;
        dropZones = new List<DropZone>();
	}

    void Update() {
        if (Hero_Management.self != null && Hero_Management.self.getSamplesCollected() > SAMPLES_NEEDED) {
            if (!active) {
                sendDropShip();
            }
        }
    }
	

    public void register(DropZone zone) {
        dropZones.Add(zone);
    }

    void sendDropShip() {
        active = true;
        System.Random r = new System.Random();
        DropZone zone = dropZones[r.Next(dropZones.Count)];
        int wait = r.Next(MIN_DROP_WAIT, MAX_DROP_WAIT);
        playRandomAudio(dropShipInbound);
        StartCoroutine(DropShip(zone, wait));
    }

    IEnumerator DropShip(DropZone zone, int wait) {
        yield return new WaitForSeconds(wait);
        System.Random r = new System.Random();
        if (r.Next(2) == 0) {
            playRandomAudio(dropShipDelayed);
            yield return new WaitForSeconds(r.Next(MIN_DROP_WAIT, MAX_DROP_WAIT));
        }
        playRandomAudio(dropShipArrived);
        zone.activate();
    }

    void playRandomAudio(AudioClip[] clips) {
        System.Random rnd = new System.Random();
        int index = rnd.Next(clips.Length);
        AudioSource.PlayClipAtPoint(clips[index], Camera.main.transform.position);
    }
}
