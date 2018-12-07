using UnityEngine;

namespace Framework.UI
{
	public class LineRenderer2D : MonoBehaviour
	{
		public Vector4[] Path;
		public Color Color;
		public AnimationCurve Curve;


		private void Start()
		{
			LineRenderer2DController.Instance.Add(this);
		}
		private void OnDisable()
		{
			LineRenderer2DController.Instance.Remove(this);
		}
	}
}
