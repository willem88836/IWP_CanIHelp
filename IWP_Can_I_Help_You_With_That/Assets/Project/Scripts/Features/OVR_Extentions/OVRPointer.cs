using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		This is basically a simplified version of Oculus' OVRGrabber.
/// </summary>
public class OVRPointer : MonoBehaviour
{
	public OVRInput.Controller ControllerType;

	private Vector3 originPosition;

	private void Awake()
	{
		// TODO: continue here with Controller stuff. 
		originPosition = transform.position;
		originRotation = transform.rotation;
	}


	// Update is called once per frame
	void Update ()
	{
		Vector3 handPosition = OVRInput.GetLocalControllerPosition(ControllerType);
		Quaternion handRotation = OVRInput.GetLocalControllerRotation(ControllerType);

		transform.gameObject
	}
}
