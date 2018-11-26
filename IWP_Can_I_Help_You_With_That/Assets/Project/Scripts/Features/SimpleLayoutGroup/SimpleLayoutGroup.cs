using System.Collections.Generic;
using UnityEngine;

namespace IWPCIH.LayoutGroup
{
	[RequireComponent(typeof(RectTransform))]
	public abstract class SimpleLayoutGroup : MonoBehaviour
	{
		public enum AnchorType { None, TopLeft, Top, TopRight, Left, Centre, Right, BottomLeft, Bottom, BottomRight };
		public readonly Dictionary<AnchorType, Vector2> Anchors = new Dictionary<AnchorType, Vector2>()
		{
			{ AnchorType.TopLeft, new Vector2(0, 1) },
			{ AnchorType.Top, new Vector2(0.5f, 1) },
			{ AnchorType.TopRight, new Vector2(1, 1) },
			{ AnchorType.Left, new Vector2(0, 0.5f) },
			{ AnchorType.Centre, new Vector2(0.5f, 0.5f) },
			{ AnchorType.Right, new Vector2(1, 0.5f) },
			{ AnchorType.BottomLeft, new Vector2(0, 0) },
			{ AnchorType.Bottom, new Vector2(0.5f, 0) },
			{ AnchorType.BottomRight, new Vector2(1, 0) },

		};


		public int Spacing;
		public bool FitChildSize;
		public AnchorType Anchor;

		protected int ChildCount { get; private set; }
		protected RectTransform Rect { get { return GetComponent<RectTransform>(); } }


		private void Update()
		{
			int updatedChildCount = transform.childCount;
			if (updatedChildCount != ChildCount)
			{
				ChildCount = updatedChildCount;
				UpdateLayoutGroup();
			}
		}

		protected abstract void UpdateLayoutGroup();

		protected void SetAnchors(RectTransform rect)
		{
			if (Anchor == AnchorType.None)
				return;

			Vector2 anchor = Anchors[Anchor];
			rect.anchorMin = anchor;
			rect.anchorMax = anchor;
		}
	}
}
