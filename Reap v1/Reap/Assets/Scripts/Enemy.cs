using UnityEngine;
using System.Collections;

public class Enemy : Character {
    Transform player;               // Reference to the player's position.
    NavMeshAgent nav;               // Reference to the nav mesh agent.
    public const int SAMPLE_COUNT = 10;
    protected int attack;

	// Use this for initialization
	protected override void Start () {
        this.gameObject.tag = "Enemy";
        player = GameObject.FindGameObjectWithTag ("Hero").transform;
        nav = GetComponent <NavMeshAgent> ();
        attack = 5;
        health = 50;
	}

    protected override void Update()
    {
        base.Update();
        if (player != null) {
            //Null check because player is null in Game Over situation.
            nav.SetDestination (player.position);
        } 
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            this.kill(Constants.DEATH_REASONS.Fighting); //For testing, just kill the enemy
        } else if (collision.gameObject.CompareTag("Hero")) {
            Hero_Management.self.injure(this.attack);
        }
    }

    protected override int getSampleCount() {
        return SAMPLE_COUNT;
    }
}