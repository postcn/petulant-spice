using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private bool init = false;
    private int framesToLive = 90;
    private int damage;

    private Vector3 origin;
    private Vector3 destination;
    private Vector3 direction;
    
	// Use this for initialization
	void Start () {
        this.gameObject.tag = "Bullet";
	}
	
	// Update is called once per frame
	void Update () {
        if (this.framesToLive == 0) {
            Destroy(this.gameObject);
            return;
        }
	    if (init)
        {
            this.transform.position = this.transform.position + this.direction;
            this.framesToLive--;
        }
	}

    void SetOrigin(Transform hero) {
        Physics.IgnoreCollision(hero.GetComponent<Collider>(), GetComponent<Collider>());
        this.origin = hero.position;
        this.origin.y += .2f;
        Vector3 angle = hero.eulerAngles;
        angle.x = 90;
        this.transform.eulerAngles = angle;
    }

    void SetDestination(Vector3 mousePoint) {
        this.destination = mousePoint;
        Vector3 heading = this.destination - this.origin;
        this.direction = heading / heading.magnitude;
        this.transform.position = this.origin;
        this.init = true;
    }

    void SetDamage(float damage) {
        this.damage = Mathf.RoundToInt(damage);
    }

    void OnCollisionEnter(Collision collision) {
        string tag = collision.gameObject.tag;
        //print("Collided with " + tag);
        if (tag == "Map" || tag == "Structure") {
            Destroy(this.gameObject);
        }
        if (tag == "Enemy") {
            collision.gameObject.SendMessage("TakeDamage", damage);
            Destroy(this.gameObject);
        }
        if (tag == "Cocoon")
        {
            collision.gameObject.SendMessage("Hatch", damage);
            Destroy(this.gameObject);
        }
    }
}
