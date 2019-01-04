using UnityEngine;

namespace IWPCIH.VRMenu
{
	public class OVRPointerTarget : MonoBehaviour
	{
		public MeshRenderer Renderer;
		[Space]
		public Material HasSelection;
		public Material HasNoSelection;

		public void HasTarget()
		{
			Renderer.material = HasSelection;
		}

		public void HasNoTarget()
		{
			Renderer.material = HasNoSelection;
		}
	}
}
