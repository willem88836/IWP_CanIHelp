using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IWPCIH;
using IWPCIH.VRMenu;

namespace IWPCIH.VRMenu
{
	/// <summary>
	///		This is basically a simplified version of Oculus' OVRGrabber.
	/// </summary>
	public class OVRPointer : MonoBehaviour
	{
		[SerializeField]
		protected bool m_parentHeldObject = false;

		// Child/attached transforms of the grabber, indicating where to snap held objects to (if you snap them).
		// Also used for ranking grab targets in case of multiple candidates.
		[SerializeField]
		protected Transform m_gripTransform = null;

		// Should be OVRInput.Controller.LTouch or OVRInput.Controller.RTouch.
		[SerializeField]
		protected OVRInput.Controller m_controller;

		[SerializeField]
		protected Transform m_parentTransform;

		protected Vector3 m_lastPos;
		protected Quaternion m_lastRot;
		protected Quaternion m_anchorOffsetRotation;
		protected Vector3 m_anchorOffsetPosition;
		protected float m_prevFlex;
		protected bool operatingWithoutOVRCameraRig = true;


		[Space, Space]
		public OVRPointerTarget PointerObject;
		public LayerMask Mask;



		protected virtual void Start()
		{
			m_lastPos = transform.position;
			m_lastRot = transform.rotation;
			if (m_parentTransform == null)
			{
				if (gameObject.transform.parent != null)
				{
					m_parentTransform = gameObject.transform.parent.transform;
				}
				else
				{
					m_parentTransform = new GameObject().transform;
					m_parentTransform.position = Vector3.zero;
					m_parentTransform.rotation = Quaternion.identity;
				}
			}
		}

		protected virtual void Awake()
		{
			// TODO: find a proper way to disable the controller that is not being used. disabling a controller doesn't work.

			m_anchorOffsetPosition = transform.localPosition;
			m_anchorOffsetRotation = transform.localRotation;

			// If we are being used with an OVRCameraRig, let it drive input updates, which may come from Update or FixedUpdate.

			OVRCameraRig rig = null;
			if (transform.parent != null && transform.parent.parent != null)
				rig = transform.parent.parent.GetComponent<OVRCameraRig>();

			if (rig != null)
			{
				rig.UpdatedAnchors += (r) => { OnUpdatedAnchors(); };
				operatingWithoutOVRCameraRig = false;
			}
		}

		// Update is called once per frame
		protected void Update()
		{
			if (operatingWithoutOVRCameraRig)
				OnUpdatedAnchors();

			ISelectable selectable;
			ShootRay(out selectable);
			Select(selectable);
		}


		protected void ShootRay(out ISelectable selectable)
		{
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, float.PositiveInfinity, Mask)) 
			{
				PointerObject.gameObject.SetActive(true);
				PointerObject.transform.position = hit.point - (transform.forward * 0.01f);
				PointerObject.transform.rotation = hit.transform.rotation;
				selectable = hit.transform.GetComponent<ISelectable>();
				
				if (selectable != null)
				{
					PointerObject.HasTarget();
				}
				else
				{
					PointerObject.HasNoTarget();
				}
			}
			else
			{
				PointerObject.gameObject.SetActive(false);
				selectable = null;
				PointerObject.HasNoTarget();
			}
		}

		protected void Select(ISelectable selectable)
		{
			if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)
				&& selectable != null)
			{
				selectable.Select();
			}
		}



		// Hands follow the touch anchors by calling MovePosition each frame to reach the anchor.
		// This is done instead of parenting to achieve workable physics. If you don't require physics on 
		// your hands or held objects, you may wish to switch to parenting.
		protected void OnUpdatedAnchors()
		{
			Vector3 handPos = OVRInput.GetLocalControllerPosition(m_controller);
			Quaternion handRot = OVRInput.GetLocalControllerRotation(m_controller);
			Vector3 destPos = m_parentTransform.TransformPoint(m_anchorOffsetPosition + handPos);
			Quaternion destRot = m_parentTransform.rotation * handRot * m_anchorOffsetRotation;
			GetComponent<Rigidbody>().MovePosition(destPos);
			GetComponent<Rigidbody>().MoveRotation(destRot);

			m_lastPos = transform.position;
			m_lastRot = transform.rotation;

			float prevFlex = m_prevFlex;
			// Update values from inputs
			m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);
		}

		protected void OnUpdatedTrigger()
		{

		}
	}
}
