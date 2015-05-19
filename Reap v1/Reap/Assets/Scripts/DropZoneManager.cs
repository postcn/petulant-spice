using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropZoneManager : MonoBehaviour {
    public const int SAMPLES_NEEDED = 250;
    public static DropZoneManager self;
    IList<DropZone> dropZones;
    private bool active = false;

    private const int MIN_DROP_WAIT = 30;
    private const int MAX_DROP_WAIT = 120;

    public AudioClip[] dropShipInbound;
    public AudioClip[] dropShipDelayed;
    public AudioClip[] dropShipArrived;

    public bool picked_up = false;
    public int delayTime = 0;

    private DropZone zone;
    private int timesDelayed = 0;
    private const int MAX_TIMES_DELAYED = 1;

	// Use this for initialization
	void Start () {
        self = this;
        dropZones = new List<DropZone>();
	}

    void Update() {
        if (Hero_Management.getSamplesCollected() > SAMPLES_NEEDED) {
            if (!active) {
                sendDropShip();
            }
        }
		if (Hero_Management.mousePlayer == null && Hero_Management.controllerPlayer == null) {
            StopCoroutine(Wait());
        }
    }
	

    public void register(DropZone zone) {
        dropZones.Add(zone);
    }

    void sendDropShip() {
        active = true;
        System.Random r = new System.Random();
        zone = dropZones[r.Next(dropZones.Count)];
        int wait = r.Next(MIN_DROP_WAIT, MAX_DROP_WAIT);
        Constants.playRandomAudio(dropShipInbound);
        delayTime = wait;
        StartCoroutine(Wait());
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(1);
        delayTime--;
        if (delayTime <=0 ) {
            System.Random r = new System.Random();
            if (r.Next(2) == 0 && timesDelayed < MAX_TIMES_DELAYED) {
                Constants.playRandomAudio(dropShipDelayed);
                delayTime = r.Next(MIN_DROP_WAIT, MAX_DROP_WAIT);
				timesDelayed++;
                StartCoroutine(Wait());
            }
            else {
                Constants.playRandomAudio(dropShipArrived);
                zone.activate();
            }
        }
        else {
            StartCoroutine(Wait());
        }
    }
}
