using UnityEngine;
using UnityEngine.UI;
using System;

namespace Framework.Core
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
	}
}
