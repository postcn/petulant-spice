using UnityEngine;
using System.Collections;

public class Hero_Management : Character {
    public static Hero_Management self;

    public const int HERO_DEATH_DELAY = 1;
    public const int HEARTBEAT_THRESHOLD = 25;
    public const int BLOODLUST_INTERVAL = 1;
    public const int BREATHING_THRESHOLD = 75;
    public const int MAX_BLOODLUST = 100;
    public const int MIN_BLOODLUST = 0;
    public const int KILL_DECREASE = 10;
    public const int MAX_HERO_HEALTH = 100;


    private const float VOLUME_STEP = 2.0f/MAX_BLOODLUST;
    private const float LIGHT_STEP = 5.0f/MAX_BLOODLUST;
    private const float LIGHT_RANGE_STEP = 40.0f/MAX_BLOODLUST;
    private const float COLOR_STEP = 1.0f/MAX_BLOODLUST;

    private int bloodlustCount = 0;
    private bool dying = false;
    public Constants.WEAPONS weapon {get; set;}

    public int getBloodlustCount() {
        return bloodlustCount;
    }

	// Use this for initialization
	protected override void Start () {
        self = this;
        StartCoroutine(Bloodlust());
        this.health = MAX_HERO_HEALTH;
        this.weapon = Constants.WEAPONS.Pistol;
	}

    private void increaseBloodlust() {
        bloodlustCount++;

        AudioSource[] sources = this.GetComponents<AudioSource>();
        foreach (AudioSource source in sources) {
            source.volume = source.volume + VOLUME_STEP;
        }

        if (bloodlustCount == HEARTBEAT_THRESHOLD) {
            sources[0].Play(); //play the breathing at greater than 50.
            sources[0].volume = 1.0f - (MAX_BLOODLUST - HEARTBEAT_THRESHOLD) * VOLUME_STEP;
        }
        if (bloodlustCount == BREATHING_THRESHOLD) {
            sources[1].Play(); //play the breathing at greater than 50.
            sources[1].volume = 1.0f - (MAX_BLOODLUST - BREATHING_THRESHOLD) * VOLUME_STEP;
        }
        changeLight(1);
    }

    public void decrementBloodlust() {
        if (bloodlustCount <= MIN_BLOODLUST) {
            return;
        }
        bloodlustCount -= KILL_DECREASE;
        
        AudioSource[] sources = this.GetComponents<AudioSource>();
        sources[0].volume = (bloodlustCount - HEARTBEAT_THRESHOLD) * VOLUME_STEP;
        sources[1].volume = (bloodlustCount - BREATHING_THRESHOLD) * VOLUME_STEP;
        
        if (bloodlustCount < HEARTBEAT_THRESHOLD) {
            sources[0].Stop();
        }
        if (bloodlustCount < BREATHING_THRESHOLD) {
            sources[1].Stop();
        }

        changeLight(-1*KILL_DECREASE);

    }

    private void changeLight(int scale) {
        Light light = Camera.main.GetComponentInChildren<Light>();
        Color newColor = light.color;
        newColor.g -= COLOR_STEP * scale;
        newColor.b -= COLOR_STEP * scale;
        light.intensity += LIGHT_STEP*scale;
        light.spotAngle += LIGHT_RANGE_STEP*scale;
        light.color = newColor; 
    }

    private bool killedByBloodlust() {
        return bloodlustCount > MAX_BLOODLUST;
    }

    public void heal(int amount) {
        this.health = Mathf.Min(this.health+amount, MAX_HERO_HEALTH);
    }

    public void resupply(int ammo) {
        //TODO: Implement the reloading and resupplying mechanics.
    }

    IEnumerator Bloodlust() {
        yield return new WaitForSeconds(BLOODLUST_INTERVAL);
        increaseBloodlust();
        if (killedByBloodlust()) {
            kill(Constants.DEATH_REASONS.Bloodlust);
        }
        else {
            StartCoroutine(Bloodlust());
        }
    }

    protected override void kill(Constants.DEATH_REASONS reason) {
        if (dying) {
            return;
        }
        dying = true;
        Debug.Log("Called Kill in Hero.");
        AudioSource[] sources = this.GetComponents<AudioSource>();
        sources[2].Play();
        StartCoroutine(Constants.Wait(HERO_DEATH_DELAY, base.kill, reason));
    }
}
