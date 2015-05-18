﻿using UnityEngine;
using System.Collections;

public class Enemy : Character {
    public Transform player;               // Reference to the player's position.
    NavMeshAgent nav;               // Reference to the nav mesh agent.
    //public const int SAMPLE_COUNT = 10;
    public int SAMPLE_COUNT = 10;
    public int attack = 5;

    public bool hasScent = true;

	// Use this for initialization
	protected override void Start () {
        this.gameObject.tag = "Enemy";
        player = GameObject.FindGameObjectWithTag ("Hero").transform;
        nav = GetComponent <NavMeshAgent> ();
	}

    protected override void Update()
    {
        base.Update();
        if (health <= 0)
        {
            this.kill(Constants.DEATH_REASONS.Fighting);
            return;
        }

        if (player != null) {
            //Null check because player is null in Game Over situation.
            
            float distance = Vector3.Distance(player.position, this.transform.position);
            if (distance < 25)
            {
                hasScent = true;
            }

            if (hasScent) {
                nav.SetDestination (player.position);
            }
        } 
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Hero")) {
            Hero_Management.self.injure(this.attack);
        }
    }

    public void TakeDamage(int damage) {
        this.health -= damage;
        print("Taking " + damage + " damage, health at " + health);
    }

    protected override int getSampleCount() {
        return SAMPLE_COUNT;
    }
}