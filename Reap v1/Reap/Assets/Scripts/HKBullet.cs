﻿using UnityEngine;
using System.Collections;

public class HKBullet : Bullet {
    
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
		if (collision.gameObject.CompareTag("Hero")) {
			collision.gameObject.SendMessage("injure",4);
		}
    }

    void SetRotation(float angle) {
        this.transform.rotation = Quaternion.Euler(90, angle, 90);
    }

    public override void SetDestination(Vector3 mousePoint)
    {
        base.SetDestination(mousePoint);
        base.direction *= 0.5f;
    }
}
