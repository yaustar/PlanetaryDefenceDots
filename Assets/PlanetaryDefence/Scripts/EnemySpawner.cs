using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour 
{
	//Enemies
	public GameObject redEnemyPrefab;
	public GameObject yellowEnemyPrefab;
	public GameObject blueEnemyPrefab;
	public GameObject testCubePrefab;
	
	public Rect spawnBoundry; //The edges of which the enemies can spawn on.

	public float redSpawnRate;
	public float yellowSpawnRate;
	public float blueSpawnRate;

	public float timeBetweenEnemySpawn;
	private float spawnTimer;

	public GameObject enemyParent;

	void Start ()
	{
		//Presetting spawn rate values.
		redSpawnRate = 5.0f;
		yellowSpawnRate = 0.0f;
		blueSpawnRate = 0.0f;
	}

	void Update ()
	{
		spawnTimer += Time.deltaTime;

		//Spawn an enemy every 'timeBetweenSpawn' seconds, if the game is running.
		if(spawnTimer >= timeBetweenEnemySpawn && Game.g.gameActive)
		{
			spawnTimer = 0.0f;
			SpawnEnemy();
			UpdateEnemySpawnRates();
		}
	}

	//Called when an enemy needs to be spawned.
	void SpawnEnemy ()
	{
		int spawnDirection = Random.Range(1, 5); //Left, Up, Right, Down.
		Vector3 spawnPos = Vector3.zero;

		//Get spawn pos based of screen direction.
		if(spawnDirection == 1)
			spawnPos = new Vector3(spawnBoundry.xMin, Random.Range(spawnBoundry.yMin, spawnBoundry.yMax), 0);
		else if(spawnDirection == 2)
			spawnPos = new Vector3(Random.Range(spawnBoundry.xMin, spawnBoundry.xMax), spawnBoundry.yMax, 0);
		else if(spawnDirection == 3)
			spawnPos = new Vector3(spawnBoundry.xMax, Random.Range(spawnBoundry.yMin, spawnBoundry.yMax), 0);
		else
			spawnPos = new Vector3(Random.Range(spawnBoundry.xMin, spawnBoundry.xMax), spawnBoundry.yMin, 0);

		//Spawn the enemy.
		var enemyPrefab = GetEnemyToSpawn(); 
		var enemyEntity = Instantiate(enemyPrefab);

		enemyEntity.transform.position = spawnPos;
		
		// Look at 0, 0, 0 along the Y axis of the enemy ship
		Vector3 direction = Vector3.zero - spawnPos;
		Quaternion targetRotation = Quaternion.FromToRotation(enemyEntity.transform.up, direction);
		enemyEntity.transform.rotation = targetRotation;
	}

	//Returns an enemy to spawn based on the spawn rates.
	GameObject GetEnemyToSpawn () {
		return redEnemyPrefab;
		
		float total = redSpawnRate + yellowSpawnRate + blueSpawnRate;
		float ranNum = Random.Range(0.0f, total);

		if(ranNum <= redSpawnRate)
			return redEnemyPrefab;

		if(ranNum <= yellowSpawnRate + redSpawnRate)
			return yellowEnemyPrefab;
		
		return blueEnemyPrefab;
	}

	//Updates the spawn rates of the enemies.
	void UpdateEnemySpawnRates ()
	{
		redSpawnRate += 0.05f;
		yellowSpawnRate += 0.03f;
		blueSpawnRate += 0.03f;
	}
}
