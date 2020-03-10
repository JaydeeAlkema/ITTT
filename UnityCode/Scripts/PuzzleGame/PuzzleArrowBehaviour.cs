using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This allows a simple Arrow gameobject to follow another object while also rotating nicely.
/// </summary>
public class PuzzleArrowBehaviour : MonoBehaviour
{
	[SerializeField] private Vector3 rotationVector;
	[SerializeField] private float rotationSpeed;

	private void Update()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		transform.Rotate(rotationVector * rotationSpeed);
	}
}
