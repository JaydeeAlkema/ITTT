using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns an asteroid at the given interval. The asteroid spawn withing the given radius.
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
	[SerializeField] private GameObject asteroidPrefab;
	[SerializeField] private float spawnInterval;
	[SerializeField] private Vector3 SpawnPlane;
	[SerializeField] private Transform asteroidParent;

	private void Start()
	{
		StartCoroutine(SpawnAsteroids());
	}

	/// <summary>
	/// Spawns an asteroid at the given interval.
	/// </summary>
	/// <returns></returns>
	private IEnumerator SpawnAsteroids()
	{
		while(true)
		{
			float posX = Random.Range(transform.position.x - SpawnPlane.x / 2, transform.position.x + SpawnPlane.x / 2);
			float posY = Random.Range(transform.position.y - SpawnPlane.y / 2, transform.position.y + SpawnPlane.y / 2);
			Instantiate(asteroidPrefab, new Vector3(posX, posY, transform.position.z), Quaternion.identity, asteroidParent);
			yield return new WaitForSeconds(spawnInterval);
		}
	}

	/// <summary>
	/// Draws an outline of where the asteroid will spawn. Purely for debugging purposes.
	/// </summary>
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, SpawnPlane);
	}
}
