using UnityEngine;

namespace IWPCIH.EditorInterfaceObjects.Features
{
	[RequireComponent(typeof(RectTransform))]
	public class FollowMouse : MonoBehaviour
	{
		public Vector3 OffSet;

		public bool Following = false;

		private RectTransform rect;


		private void Awake()
		{
			rect = GetComponent<RectTransform>();
		}

		public void Update()
		{
			if (Following)
			{
				rect.transform.position = Input.mousePosition + OffSet;
			}
		}
	}
}
