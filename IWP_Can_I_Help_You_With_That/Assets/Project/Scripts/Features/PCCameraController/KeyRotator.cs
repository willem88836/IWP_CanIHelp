using UnityEngine;

namespace IWPCIH.CameraRotator
{
	public sealed class KeyRotator : Rotator
	{
#if UNITY_EDITOR
		private void Update()
		{
			Vector3 dir = Vector3.zero;

			dir.x = -Input.GetAxis("Vertical");
			dir.y = Input.GetAxis("Horizontal");
			Rotate(dir);
		}
#endif
	}
}
