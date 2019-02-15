using UnityEngine;

namespace IWPCIH
{
	public class TransformSingleton : MonoBehaviour
	{
		public static Transform Instance;

		private void Awake()
		{
			if (Instance != null)
				Debug.LogWarning("Overriding TransformSingleton!");

			Instance = transform;
		}
	}
}
