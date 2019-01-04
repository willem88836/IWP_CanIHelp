using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	public Camera TargetCamera = null;
	[Range(0, 1000)] public float Distance = 10;
	[Range(0, 1)] public float RotationLerpSpeed = 0.8f;
	[Range(0, 1)] public float MovementLerpSpeed = 0.8f;



	// Update is called once per frame
	void Update ()
	{
		Vector3 currentPos = transform.position;
		Quaternion currentRot = transform.rotation;

		Quaternion targetRot = TargetCamera.transform.rotation;
		Vector3 targetPos =
			TargetCamera.transform.position
			+ TargetCamera.transform.forward * Distance;

		targetRot = Quaternion.Lerp(currentRot, targetRot, RotationLerpSpeed);
		targetPos = Vector3.Lerp(currentPos, targetPos, MovementLerpSpeed);

		transform.rotation = targetRot;
		transform.position = targetPos;
	}
}
