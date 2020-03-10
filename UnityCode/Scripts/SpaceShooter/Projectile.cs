using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectile behaviour.
/// Speaks for itself to be honest.
/// </summary>
public class Projectile : MonoBehaviour
{
	[SerializeField] private int damagePoints = 250;
	[SerializeField] private Rigidbody rb;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float destroyTimer;

	public int DamagePoints { get => damagePoints; set => damagePoints = value; }

	private void Start()
	{
		Destroy(gameObject, destroyTimer);
		rb.AddForce(new Vector3(transform.position.x, transform.position.y, transform.position.z * moveSpeed), ForceMode.Impulse);
	}
}
