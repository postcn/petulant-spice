using UnityEngine;
using System.Collections;

public class Cocoon : MonoBehaviour {

    public GameObject enemyToHatch;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void CheckAll(Vector3 position, float radius) {

    }

    void CheckHatchFireBullet(Vector3 position) {
        this.CheckHatch(position, 5.5);
    }

    void CheckHatchMovement(Vector3 position) {
        this.CheckHatch(position, 3);
    }

    void CheckHatch(Vector3 position, double radius) {
        float distance = Vector3.Distance(position, this.transform.position);
        
        if (distance < radius)
        {
            Hatch(0);
        }
    }

    public void Hatch(int damage) {
        GameObject enemy = (GameObject) Instantiate (enemyToHatch, this.transform.position, this.transform.rotation);
        enemy.GetComponent<Enemy>().health = enemy.GetComponent<Enemy>().getHealth() - damage;
        Destroy(this.gameObject);
    }

}
