using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Asteroid behaviour.
/// </summary>
public class Asteroid : MonoBehaviour
{
	[SerializeField] private int hitPoints = 1000;
	[SerializeField] private float damageOnHit = 25;
	[SerializeField] private float pointsWorth = 100;
	[SerializeField] private float fuelWorth = 10;
	[Space]
	[SerializeField] private Rigidbody rb;
	[SerializeField] private float moveSpeed;
	[SerializeField] private Vector3 scale;
	[Space]
	[SerializeField] private float minWidth;
	[SerializeField] private float maxWidth;
	[SerializeField] private float minHeight;
	[SerializeField] private float maxHeight;
	[Space]
	[SerializeField] private float animSpeed;
	[SerializeField] private bool canAnimate = true;
	[Space]
	[SerializeField] private GameObject deathParticles;
	[Space]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip explosionAudio;
	[Space]
	[SerializeField] private bool canDie = true;

	/// <summary>
	/// gives Asteroid initial poush, scale, rotation and how much it's worth to destroy.
	/// </summary>
	private void Start()
	{
		rb.AddForce(Vector3.back * moveSpeed, ForceMode.Impulse);
		scale.x = Random.Range(minWidth, maxWidth); scale.y = Random.Range(minHeight, maxHeight); scale.z = Random.Range(1, 3);
		transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
		transform.localScale = Vector3.zero;
		pointsWorth *= (scale.x + scale.y / 2);
	}

	/// <summary>
	/// Checks for some simple things. When to animate and when to die :)
	/// </summary>
	private void Update()
	{
		if(canAnimate)
			SpawnAnimation();
		if(canDie && hitPoints <= 0)
		{
			canDie = false;
			UIManager.Instance.AddPoints((int)pointsWorth);
			GameObject.FindGameObjectWithTag("SpaceShooterPlayer").GetComponent<SpaceShooterPlayer>().AddFuel(fuelWorth);
			DeathEvent();
		}
	}

	/// <summary>
	/// Checks for collision with any projectiles and or the player.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Projectile"))
		{
			hitPoints -= other.GetComponent<Projectile>().DamagePoints;
			Destroy(other.gameObject);
		}
		if(other.CompareTag("SpaceShooterPlayer"))
		{
			other.GetComponent<SpaceShooterPlayer>().HitPointsLeft -= damageOnHit;
			other.GetComponent<SpaceShooterPlayer>().HitAnimationActive = true;
			DeathEvent();
		}
	}

	/// <summary>
	/// The starting animation of the asteroid getting bigger until it reaches it's scale.
	/// Just looks nicer than spawning an Asteroid.
	/// </summary>
	private void SpawnAnimation()
	{
		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scale.x, scale.y, scale.z), animSpeed * Time.deltaTime);
		if(transform.localScale.x >= scale.x && transform.localScale.y >= scale.y && transform.localScale.z >= scale.z)
			canAnimate = false;
	}

	/// <summary>
	/// The DeathEvent. Spawns nice particles plays a cool audioclip and disables this object. Eventually destroys it aswell.
	/// </summary>
	private void DeathEvent()
	{
		audioSource.PlayOneShot(explosionAudio);

		Instantiate(deathParticles, transform.position, Quaternion.identity, transform);

		gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.GetComponent<MeshCollider>().enabled = false;

		Destroy(gameObject, 5f);
	}
}
