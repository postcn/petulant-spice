using UnityEngine;
using System.Collections;

public class DaleManagement : MonoBehaviour {
    public const int MIN_TIME_BEFORE_DELAY = 60;
    public const int MAX_TIME_BEFORE_DELAY = 360;
    public const int MIN_TIME_BEFORE_JOKE = 60;
    public const int MAX_TIME_BEFORE_JOKE = 240;

    public AudioClip[] delayClips;
    public AudioClip[] jokeClips;
    public AudioClip[] firedClips;
    private int[] delayClipsPlayTimes;
    private int[] jokeClipsPlayTimes;
    private int mostPlayedDelay = 0;
    private int mostPlayedJoke = 0;
    private bool fired = false;
    private IEnumerator delay;
    private IEnumerator joke;

    public static DaleManagement self;

    public int delayTime = 0;

    public bool isAFK() {
        return delayTime > 0;
    }

	// Use this for initialization
	void Start () {
        self = this;
        delayClipsPlayTimes = new int[delayClips.Length];
        jokeClipsPlayTimes = new int[jokeClips.Length];
        delay = Delay();
        joke = Joke();
        StartCoroutine(delay);
        StartCoroutine(joke);
	}
	
	// Update is called once per frame
	void Update () {
	    if (fired || DropZoneManager.self.picked_up) {
            StopCoroutine(delay);
            StopCoroutine(joke);
        }
	}

    public void Fired() {
        if ((Hero_Management.mousePlayer == null || Hero_Management.controllerPlayer == null) && !DropZoneManager.self.picked_up){
			Constants.playRandomAudio(firedClips);
			if (Hero_Management.mousePlayer == null && Hero_Management.controllerPlayer == null) {
				fired = true;
			}
        }
    }

    IEnumerator Delay() {
        if (delayTime == 0) {
            int daleSeconds = Random.Range(MIN_TIME_BEFORE_DELAY, MAX_TIME_BEFORE_DELAY);
            Debug.Log("Dale will be delayed in " + daleSeconds);
            yield return new WaitForSeconds(daleSeconds);
            int delaySoundIndex = Random.Range(0, delayClips.Length);
            while (delaySoundIndex == mostPlayedDelay) {
                delaySoundIndex = Random.Range(0, delayClips.Length);
            }
            delayClipsPlayTimes[delaySoundIndex]++;
            if (delayClipsPlayTimes[delaySoundIndex] > delayClipsPlayTimes[mostPlayedDelay]) {
                mostPlayedDelay = delaySoundIndex;
            }
            AudioClip clip = delayClips[delaySoundIndex];
            delayTime = Mathf.Max(Mathf.RoundToInt(clip.length), MIN_TIME_BEFORE_DELAY);
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        } else {
            yield return new WaitForSeconds(1);
        }
        delayTime--;

        StartCoroutine(Delay());
    }

    IEnumerator Joke() {
        int jokeSeconds = Random.Range(MIN_TIME_BEFORE_JOKE, MAX_TIME_BEFORE_JOKE);
        yield return new WaitForSeconds(jokeSeconds);
        if (delayTime > 0) {
            StartCoroutine(Joke());
        } else {
            int jokeSoundIndex = Random.Range(0, jokeClips.Length);
            while (jokeSoundIndex == mostPlayedJoke) {
                jokeSoundIndex = Random.Range(0, jokeClips.Length);
            }
            jokeClipsPlayTimes[jokeSoundIndex]++;
            if (jokeClipsPlayTimes[jokeSoundIndex] > jokeClipsPlayTimes[mostPlayedJoke]) {
                mostPlayedJoke = jokeSoundIndex;
            }
            AudioClip clip = jokeClips[jokeSoundIndex];
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            yield return new WaitForSeconds(Mathf.RoundToInt(clip.length));
            StartCoroutine(Joke());
        }
    }
}
