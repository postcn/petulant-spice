using UnityEngine;
using System.Collections;

public class Hero_Management : Character {

    public static int BLOODLUST_INTERVAL = 1;
    public static int BREATHING_THRESHOLD = 50;
    public static int MAX_BLOODLUST = 100;

    private int bloodlustCount = 0;
    private static float volumeStep = 2.0f/MAX_BLOODLUST;
    private static float LIGHT_STEP = 5.0f/MAX_BLOODLUST;
    private static float LIGHT_RANGE_STEP = 40.0f/MAX_BLOODLUST;
    private static float COLOR_STEP = 0.01f;


	// Use this for initialization
	void Start () {
        StartCoroutine(Bloodlust());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void increaseBloodlust() {
        bloodlustCount++;

        AudioSource[] sources = this.GetComponents<AudioSource>();
        foreach (AudioSource source in sources) {
            source.volume = source.volume + volumeStep;
        }

        if (bloodlustCount == BREATHING_THRESHOLD) {
            sources[1].Play(); //play the breathing at greater than 50.
            sources[1].volume = 0.0f;
        }


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
