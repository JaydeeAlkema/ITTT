using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is only used in the puzzlegame. the game that isn't actually being developed on atm.
/// </summary>
public class ArduinoOutputTest : MonoBehaviour
{
	[SerializeField] private Arduino arduino;
	[SerializeField] private Vector3 rot;
	[SerializeField] private Vector3 deadZone;
	[SerializeField] private float rotationSmoothing;

	/// <summary>
	/// Get and parse the rotation of the MPU6050.
	/// </summary>
	private void Update()
	{
		rot = arduino.Rotation;

		if(Input.GetKey(KeyCode.D))
			rot.z = -45;
		if(Input.GetKey(KeyCode.A))
			rot.z = 45;
		if(Input.GetKey(KeyCode.W))
			rot.x = 45;
		if(Input.GetKey(KeyCode.S))
			rot.x = -45;

		transform.rotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rot), rotationSmoothing * Time.deltaTime);
	}
}
