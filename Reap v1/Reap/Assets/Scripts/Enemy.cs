using UnityEngine;
using System.Collections;

public class Enemy : Character {

	// Use this for initialization
	protected override void Start () {
        StartCoroutine(Bloodlust());
	}

    //TODO: Remove when testing i
    IEnumerator Bloodlust() {
        yield return new WaitForSeconds(Random.Range(85, 100));
        kill(Constants.DEATH_REASONS.Fighting);
    }

}
