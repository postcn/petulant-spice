using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	private int numSpawned = 0;
	public GameObject[] spawnEnemies;       // The enemy prefabs that can be spawned
	public float spawnTime = 3f;            // How long between each spawn.
	public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
	
	
	void Start ()
	{
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	
	
	void Spawn ()
	{
		// If the player has no health left...
		if(numSpawned >= 3)
		{
			// ... exit the function.
			return;
		}

		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		int spawnEnemyIndex = Random.Range (0, spawnEnemies.Length);
		
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		Instantiate (spawnEnemies[spawnEnemyIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

		numSpawned++;
	}
}