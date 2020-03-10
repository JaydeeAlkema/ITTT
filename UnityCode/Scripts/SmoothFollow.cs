using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERA_MODE
{
	LERP,
	SMOOTHDAMP
}

/// <summary>
/// A smooth cam follow script. This has two ways of follow. one with SmoothDamp, and one using Lerp.
/// Both have different end results.
/// </summary>
public class SmoothFollow : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private Transform myT;
	[SerializeField] private float smoothDampDistanceDamp;
	[SerializeField] private float lerpDistanceDamp;
	[SerializeField] private Vector3 defaultDistance;
	[SerializeField] private Vector3 velocity = Vector3.one;
	[Space]
	[SerializeField]
	private CAMERA_MODE camMode;

	Vector3 currentPos;

	private void LateUpdate()
	{
		switch(camMode)
		{
			case CAMERA_MODE.LERP:
				LerpFollow();
				break;
			case CAMERA_MODE.SMOOTHDAMP:
				SmoothDampFollow();
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Follow the target using SmoothDamp.
	/// </summary>
	private void SmoothDampFollow()
	{
		Vector3 desiredPos = target.position + defaultDistance;
		currentPos = Vector3.SmoothDamp(myT.position, desiredPos, ref velocity, smoothDampDistanceDamp);
		myT.position = currentPos;
	}

	/// <summary>
	/// Follow the target using Lerp.
	/// </summary>
	private void LerpFollow()
	{
		Vector3 desiredPos = target.position + defaultDistance;
		currentPos = Vector3.Lerp(myT.position, desiredPos, lerpDistanceDamp * Time.deltaTime);
		myT.position = currentPos;
	}
}
