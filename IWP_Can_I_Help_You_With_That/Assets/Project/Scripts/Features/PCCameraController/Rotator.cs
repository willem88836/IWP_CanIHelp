using UnityEngine;

namespace IWPCIH.CameraRotator
{
	public abstract class Rotator : MonoBehaviour
	{
		public float Speed;

		public void Rotate(Vector3 direction)
		{
			direction.Normalize();
			direction *= Speed;

			if (Mathf.Abs(transform.rotation.x) > 0.6f
				&& ((transform.rotation.x < 0 & direction.x < 0)
				|| (transform.rotation.x > 0 & direction.x > 0)))
					direction.x = 0;

			transform.Rotate(new Vector3(direction.x, 0, 0), Space.Self);
			transform.Rotate(new Vector3(0, direction.y, 0), Space.World);
		}
	}
}
