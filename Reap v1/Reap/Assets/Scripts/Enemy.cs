using UnityEngine;
using System.Collections;

public class Enemy : Character {
    NavMeshAgent nav;               // Reference to the nav mesh agent.
    public int SAMPLE_COUNT = 10;
    public int attack = 5;
    public const int FRAME_UPDATE_COUNT = 15;

    public bool hasScent = true;
    private int framesToUpdate = 0;
    private Hero_Management last;

	// Use this for initialization
	protected override void Start () {
        this.gameObject.tag = "Enemy";
        nav = GetComponent <NavMeshAgent> ();
	}

    protected override void Update()
    {
        base.Update();

        if (framesToUpdate > 0) {
            framesToUpdate--;
            return;
        }
        else {
            framesToUpdate = FRAME_UPDATE_COUNT;
        }

		Hero_Management player = Hero_Management.closestPlayer(this.transform.position);
        if (player != null && this.health > 0) {
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

    protected override void kill(Constants.DEATH_REASONS reason) {
        if (last != null) {
            last.decrementBloodlust();
            Hero_Management.addSamples(this.getSampleCount());
        }
        base.kill(reason);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Hero")) {
			collision.gameObject.SendMessage("injure",this.attack);
        }
    }

    public void TakeDamage(int damage) {
        this.health -= damage;
    }

    public void LastHero(Hero_Management hero) {
        this.last = hero;
    }

    protected override int getSampleCount() {
        return SAMPLE_COUNT;
    }
}