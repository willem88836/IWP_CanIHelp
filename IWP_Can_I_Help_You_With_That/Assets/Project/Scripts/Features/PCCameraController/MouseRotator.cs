using UnityEngine;

namespace IWPCIH.CameraRotator
{
	public sealed class MouseRotator : Rotator
	{
		private Vector3 previous;


		private void Awake()
		{
			previous = Input.mousePosition;
		}

		private void Update()
		{
			if (Input.GetMouseButton(0))
			{
				Vector3 current = Input.mousePosition;
				Vector3 delta = current - previous;
				delta = new Vector3(delta.y, -delta.x, 0);	
				Rotate(delta);
				previous = current;
			}
		}
	}
}
