using UnityEngine;
using System.Collections;

public class Enemy : Character {
    NavMeshAgent nav;               // Reference to the nav mesh agent.
    public int SAMPLE_COUNT = 10;
    public int attack = 5;

    public bool hasScent = true;

	// Use this for initialization
	protected override void Start () {
        this.gameObject.tag = "Enemy";
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

		Hero_Management player = Hero_Management.closestPlayer(this.transform.position);
        if (player != null) {
            //Null check because player is null in Game Over situation.
            
            float distance = Vector3.Distance(player.gameObject.transform.position, this.transform.position);
            if (distance < 25)
            {
                hasScent = true;
            }

            if (hasScent) {
                nav.SetDestination (player.gameObject.transform.position);
            }
        } 
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Hero")) {
			collision.gameObject.SendMessage("injure",this.attack);
        }
    }

    public void TakeDamage(int damage) {
        this.health -= damage;
    }

    protected override int getSampleCount() {
        return SAMPLE_COUNT;
    }
}