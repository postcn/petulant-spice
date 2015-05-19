using UnityEngine;
using System.Collections;

public class HKEnemy : Enemy {

    private static float fireRate = 1.5f;
    public GameObject bulletPrefab;
    public AudioClip weaponFire;
    
    protected override void Start () {
        base.Start();
        InvokeRepeating ("Shoot", fireRate, fireRate);
    }
    
    void OnCollisionEnter(Collision collision) {
        //do nothing
    }
    
    void Shoot() {
		Hero_Management player = Hero_Management.closestPlayer(this.transform.position);
        if (player == null) {
            return;
        }
        float distance = Vector3.Distance(player.gameObject.transform.position, this.transform.position);
        
        if (distance < 7)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SendMessage("SetOrigin", this.transform);
			bullet.SendMessage("SetDestination", player.gameObject.transform.position);
            bullet.SendMessage("SetDamage", 2);
			bullet.SendMessage("SetRotation", Vector3.Angle(player.gameObject.transform.position, this.transform.position));

            AudioSource.PlayClipAtPoint(this.weaponFire, this.transform.position);
        }
    }
}