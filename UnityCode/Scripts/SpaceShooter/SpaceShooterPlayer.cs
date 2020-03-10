using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The SpaceShooterPlayer behaviour.
/// Handles everything from: Rotation, movement and shooting.
/// </summary>
public class SpaceShooterPlayer : MonoBehaviour
{
	[SerializeField] private bool debugMode;
	[Space]
	[SerializeField] private Arduino arduino;
	[SerializeField] private Rigidbody rb;
	[SerializeField] private GameObject graphicParent;
	[Space]
	[SerializeField] private float fuelLeft;
	[SerializeField] private float fuelDepletionTick;
	[SerializeField] private float hitPointsLeft;
	[SerializeField] private float hitAnimationInterval;
	[SerializeField] private bool hitAnimationActive = false;
	[Space]
	[SerializeField] private float rotSmoothing;
	[Space]
	[SerializeField] private float moveSpeed;
	[SerializeField] private float moveSpeedMultiplier;
	[SerializeField] private float moveSpeedThreshold;
	[Space]
	[SerializeField] private bool canShoot = true;
	[SerializeField] private float shootInterval;
	[SerializeField] private Transform projectileParent;
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform[] muzzles;
	[SerializeField] private int muzzleIndex;
	[Space]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip shootAudio;
	[SerializeField] private AudioClip hitAudio;

	float posX;

	public float FuelLeft { get => fuelLeft; set => fuelLeft = value; }
	public float HitPointsLeft { get => hitPointsLeft; set => hitPointsLeft = value; }
	public bool HitAnimationActive { get => hitAnimationActive; set => hitAnimationActive = value; }

	private void Start()
	{
		StartCoroutine(DepleteFuel());
	}

	private void Update()
	{
		float gyroAccel = arduino.GyroAccel;
		RotateWithArduinoRot();
		if(canShoot)
			if(Input.GetKeyDown(KeyCode.Space) || gyroAccel >= 75f)
			{
				StartCoroutine(Shoot());
				canShoot = false;
			}
		if(hitAnimationActive)
		{
			hitAnimationActive = false;
			StartCoroutine(HitAnimation());
		}
	}

	/// <summary>
	/// We have to move the player in fixed update because we are using a Rigidbody.
	/// </summary>
	private void FixedUpdate()
	{
		rb.MovePosition(new Vector3(posX, transform.position.y, transform.position.z));
	}

	/// <summary>
	/// This function gets the rotation from the Arduino component and applies it to the player for visual feedback.
	/// We also use the rotation as an indication on how fast the player should move left or right.
	/// </summary>
	private void RotateWithArduinoRot()
	{
		float arduinoRot = arduino.Rotation.z;

		posX -= arduinoRot * Time.deltaTime;
		moveSpeed = Mathf.Clamp(arduinoRot * -moveSpeedMultiplier, -moveSpeedThreshold, moveSpeedThreshold);

		transform.rotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, arduinoRot), rotSmoothing * Time.deltaTime);
	}

	/// <summary>
	/// Shoots a projectile from one of the 4 muzzles. Each time it's random, just because it looks cool :)
	/// You can shoot with space (although this was for testing purposes), or to jolt the controller back, simulating recoil, the player can shoot.
	/// </summary>
	private IEnumerator Shoot()
	{
		audioSource.volume = 0.2f;
		audioSource.PlayOneShot(shootAudio);
		muzzleIndex = Random.Range(0, muzzles.Length);
		Instantiate(projectilePrefab, muzzles[muzzleIndex].position, Quaternion.identity, projectileParent);
		yield return new WaitForSeconds(shootInterval);
		canShoot = true;
	}

	/// <summary>
	/// A cheap way of animating an hit animation. Works perfect non the less
	/// </summary>
	/// <returns></returns>
	private IEnumerator HitAnimation()
	{
		audioSource.volume = 1f;
		audioSource.PlayOneShot(hitAudio);
		graphicParent.SetActive(false);
		yield return new WaitForSeconds(hitAnimationInterval);
		graphicParent.SetActive(true);
		yield return new WaitForSeconds(hitAnimationInterval);
		graphicParent.SetActive(false);
		yield return new WaitForSeconds(hitAnimationInterval);
		graphicParent.SetActive(true);
		yield return new WaitForSeconds(hitAnimationInterval);
		graphicParent.SetActive(false);
		yield return new WaitForSeconds(hitAnimationInterval);
		graphicParent.SetActive(true);
		yield return new WaitForSeconds(hitAnimationInterval);
		graphicParent.SetActive(false);
		yield return new WaitForSeconds(hitAnimationInterval);
		graphicParent.SetActive(true);
		yield return new WaitForSeconds(hitAnimationInterval);
		graphicParent.SetActive(false);
		yield return new WaitForSeconds(hitAnimationInterval);
		graphicParent.SetActive(true);
		yield return new WaitForSeconds(hitAnimationInterval);
	}

	/// <summary>
	/// Keep depleting fuel. (Just like in real life)
	/// </summary>
	/// <returns></returns>
	private IEnumerator DepleteFuel()
	{
		while(true)
		{
			yield return new WaitForSeconds(fuelDepletionTick);
			fuelLeft -= 1;
		}
	}

	/// <summary>
	/// Gives the option from other scripts to simpely add fuel (actually only the Asteroid usess this.)
	/// </summary>
	/// <param name="fuelToAdd"></param>
	public void AddFuel(float fuelToAdd)
	{
		fuelLeft += fuelToAdd;
		if(fuelLeft > 100)
			fuelLeft = 100;
	}
}
