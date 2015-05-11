using UnityEngine;
using System.Collections;

public class Enemy : Character {
    Transform player;               // Reference to the player's position.
    NavMeshAgent nav;               // Reference to the nav mesh agent.
    public const int SAMPLE_COUNT = 10;
    protected int attack;

    public bool hasScent = true;

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

            float playerX = player.position.x;
            float playerY = player.position.y;
            float enemyX = this.transform.position.x;
            float enemyY = this.transform.position.y;
            
            float distance = Mathf.Sqrt(Mathf.Pow((playerX - enemyX), 2) + Mathf.Pow((playerY - enemyY), 2));
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