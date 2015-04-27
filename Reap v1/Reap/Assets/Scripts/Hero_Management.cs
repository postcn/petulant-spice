using UnityEngine;
using System.Collections;

public class Hero_Management : Character {

    public static int BLOODLUST_INTERVAL = 1;

    private int bloodlustCount = 0;
    private float volumeStep = 0.02f;
    private float LIGHT_STEP = 5.0f/100.0f;
    private float LIGHT_RANGE_STEP = 40.0f/100.0f;
    private float COLOR_STEP = 0.01f;


	// Use this for initialization
	void Start () {
        StartCoroutine(Bloodlust());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void increaseBloodlust() {
        bloodlustCount++;

        AudioSource source = this.GetComponent<AudioSource>();
        source.volume = source.volume + volumeStep;

        Light light = Camera.main.GetComponentInChildren<Light>();
        light.intensity = light.intensity + LIGHT_STEP;
        Color newColor = light.color;
        newColor.g -= COLOR_STEP;
        newColor.b -= COLOR_STEP;
        light.color = newColor;

        light.spotAngle += LIGHT_RANGE_STEP;
    }

    private bool killedByBloodlust() {
        return bloodlustCount > 100;
    }

    IEnumerator Bloodlust() {
        yield return new WaitForSeconds(BLOODLUST_INTERVAL);
        increaseBloodlust();
        if (killedByBloodlust()) {
            kill();
        }
        StartCoroutine(Bloodlust());
    }
}
