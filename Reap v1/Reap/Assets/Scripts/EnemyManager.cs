using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject hero;
	private int numSpawned = 0;
	public GameObject[] spawnEnemies;       // The enemy prefabs that can be spawned
	public float spawnTime = 3f;            // How long between each spawn.
	public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
	
	
	void Start ()
	{
        for (int i = 0; i < spawnEnemies.Length; i++)
        {
            spawnEnemies[i].GetComponent<Enemy>().hasScent = true;
        }

		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	
	
	void Spawn ()
	{
		// If the player has no health left..
        if (Hero_Management.self == null) {
            //Hero is dead.
            return;
        }

		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		int spawnEnemyIndex = Random.Range (0, spawnEnemies.Length);

        float playerX = hero.transform.position.x;
        float playerY = hero.transform.position.y;
        float spawnX = spawnPoints [spawnPointIndex].position.x;
        float spawnY = spawnPoints [spawnPointIndex].position.y;

        float distance = Mathf.Sqrt(Mathf.Pow((playerX - spawnX), 2) + Mathf.Pow((playerY - spawnY), 2));
        if (distance < 25)
        {
            return;
        }

        //GameObject enemy;
		
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate (spawnEnemies[spawnEnemyIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

        if (spawnEnemyIndex == 3)
        {
            for (int i = 0; i < 4; i++) {
                Instantiate (spawnEnemies[spawnEnemyIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            }
        }

		numSpawned++;
	}
}