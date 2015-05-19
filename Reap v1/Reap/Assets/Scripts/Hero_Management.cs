using UnityEngine;
using System.Collections;

public class Hero_Management : Character {
    public static Hero_Management mousePlayer;
	public static Hero_Management controllerPlayer;

    public const int HERO_DEATH_DELAY = 1;
    public const int HEARTBEAT_THRESHOLD = 25;
    public const int BLOODLUST_INTERVAL = 1;
    public const int BREATHING_THRESHOLD = 75;
    public const int MAX_BLOODLUST = 100;
    public const int MIN_BLOODLUST = 0;
    public const int KILL_DECREASE = 1;
    public const int MAX_HERO_HEALTH = 100;
    public const int MAX_START_AMMO = 300;

    public AudioClip[] injurySounds;
    public AudioSource emptyMagazinePlayer;
    public AudioClip weaponFire;
	public bool isMousePlayer;
	public Camera followingCamera;

    private const float VOLUME_STEP = 2.0f/MAX_BLOODLUST;
    private const float LIGHT_STEP = 5.0f/MAX_BLOODLUST;
    private const float LIGHT_RANGE_STEP = 40.0f/MAX_BLOODLUST;
    private const float COLOR_STEP = 1.0f/MAX_BLOODLUST;

    private static int samplesCollected = 0;
    private int bloodlustCount = 0;
    private bool dying = false;
    private int decreasedDelay = 2;
    private int ammo;
    public Constants.WEAPONS weapon {get; set;}

    public static int getSamplesCollected() {
        return samplesCollected;
    }

    public static void addSamples(int sampleAmount) {
        samplesCollected += sampleAmount;
    }

    public static void removeSamples(int decreaseAmount) {
        samplesCollected -= decreaseAmount;
    }

    public int getBloodlustCount() {
        return bloodlustCount;
    }

	public static Hero_Management closestPlayer(Vector3 location) {
		if (mousePlayer == null && controllerPlayer == null) {
			return null;
		}
		else if (mousePlayer ==null) {
			return controllerPlayer;
		}
		else if (controllerPlayer == null) {
			return mousePlayer;
		}
		else {
			float mDist = Vector3.Distance(location, mousePlayer.gameObject.transform.position);
			float cDist = Vector3.Distance(location, controllerPlayer.gameObject.transform.position);
			if (mDist > cDist) {
				return mousePlayer;
			}
			else {
				return controllerPlayer;
			}
		}
	}

	public static void maximizeCamera() {
		Debug.Log("Maximized.");
		if (mousePlayer == null && controllerPlayer != null) {
			maximizeCamera(controllerPlayer);
		} 
		else if (controllerPlayer == null && mousePlayer != null) {
			maximizeCamera(mousePlayer);
		}
	}

	private static void maximizeCamera(Hero_Management alive) {
		alive.followingCamera.rect = new Rect(0,0,1,1);
	}

	// Use this for initialization
	protected override void Start () {
        if (isMousePlayer) {
			mousePlayer = this;
		} else {
			controllerPlayer = this;
		}
        StartCoroutine(Bloodlust());
        this.health = MAX_HERO_HEALTH;
        this.weapon = Constants.WEAPONS.Pistol;
        this.ammo = MAX_START_AMMO;
	}

    private void increaseBloodlust() {
        bloodlustCount++;

        AudioSource[] sources = this.GetComponents<AudioSource>();
        foreach (AudioSource source in sources) {
            source.volume = source.volume + VOLUME_STEP*2;
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

    public void clearBloodlust() {
        changeLight(-1*bloodlustCount);
        bloodlustCount = 0;
        updateBloodlustComponents();
    }

    public void decrementBloodlust() {
        if (bloodlustCount <= MIN_BLOODLUST) {
            return;
        }
        if (decreasedDelay > 0) {
            decreasedDelay--;
            return;
        }
        else {
            decreasedDelay = 2;
        }
        bloodlustCount -= KILL_DECREASE;
        
        updateBloodlustComponents();
        changeLight(-1*KILL_DECREASE);

    }

    private void updateBloodlustComponents() {
        AudioSource[] sources = this.GetComponents<AudioSource>();
        sources[0].volume = (bloodlustCount - HEARTBEAT_THRESHOLD) * VOLUME_STEP;
        sources[1].volume = (bloodlustCount - BREATHING_THRESHOLD) * VOLUME_STEP;
        
        if (bloodlustCount < HEARTBEAT_THRESHOLD) {
            sources[0].Stop();
        }
        if (bloodlustCount < BREATHING_THRESHOLD) {
            sources[1].Stop();
        }
    }

    private void changeLight(int scale) {
        Light light = followingCamera.GetComponentInChildren<Light>();
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

    public void injure(int amount) {
        this.health -= amount;
        Constants.playRandomAudio(injurySounds);
    }

    public void resupply(int ammo) {
        this.ammo += ammo;
    }

    public bool fireWeapon(int count) {
        if (weapon == Constants.WEAPONS.Pistol) {
            AudioSource.PlayClipAtPoint(this.weaponFire, this.transform.position);
            return true;
        }
        if (count <= ammo) {
            AudioSource.PlayClipAtPoint(this.weaponFire, this.transform.position);
            this.ammo -= count;
            return true;
        }
        if (!emptyMagazinePlayer.isPlaying) {
            emptyMagazinePlayer.Play();
        }
        return false;
    }

    public int getCurrentAmmunition() {
        return this.ammo;
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
		if (this.Equals (mousePlayer)) {
			mousePlayer = null;
		}
		else {
			controllerPlayer = null;
		}
        dying = true;
        Debug.Log("Called Kill in Hero.");
        AudioSource[] sources = this.GetComponents<AudioSource>();
        sources[2].Play();
        StartCoroutine(Constants.Wait(HERO_DEATH_DELAY, base.kill, reason));
    }

    public void pickUp() {
        base.kill(Constants.DEATH_REASONS.DropShip);
    }
}
