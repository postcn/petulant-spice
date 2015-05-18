using UnityEngine;
using System.Collections;

public class HKBullet : Bullet {
    
    private bool init = false;
    private int framesToLive = 90;
    private int damage;
    
    private Vector3 origin;
    private Vector3 destination;
    private Vector3 direction;
    
    // Use this for initialization
    void Start () {
        this.gameObject.tag = "HKBullet";
    }
    
    void OnCollisionEnter(Collision collision) {
        string tag = collision.gameObject.tag;
        //print("Collided with " + tag);
        if (tag == "Map" || tag == "Structure") {
            Destroy(this.gameObject);
        }
        if (tag == "Hero") {
            Hero_Management.self.injure(2);
            Destroy(this.gameObject);
        }
    }

    void SetRotation(float angle) {
        this.transform.rotation = Quaternion.Euler(90, angle, 90);
    }
}
