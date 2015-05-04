using UnityEngine;
using System.Collections;

public class Enemy : Character {

	// Use this for initialization
	protected override void Start () {
        this.gameObject.tag = "Enemy";
        StartCoroutine(Bloodlust());
	}

    //TODO: Remove when testing i
    IEnumerator Bloodlust() {
        yield return new WaitForSeconds(Random.Range(85, 100));
        kill(Constants.DEATH_REASONS.Fighting);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(this.gameObject); //For testing, just kill the enemy
            Debug.Log("Killed Enemy");
        }
    }
}