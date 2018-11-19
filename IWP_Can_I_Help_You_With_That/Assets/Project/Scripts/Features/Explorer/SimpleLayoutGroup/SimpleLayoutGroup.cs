using UnityEngine;

namespace IWPCIH.Explorer
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public abstract class SimpleLayoutGroup : MonoBehaviour
	{
		public enum HorizontalAllignment { Left, Centre, Right }
		public enum VerticalAllignment { Top, Centre, Bottom }


		public HorizontalAllignment horizontalAllignment;
		public VerticalAllignment verticalAlligment;
		public float Spacing;


		private int previousChildCount = 0;
		private RectTransform rect;

		private void Awake()
		{
			rect = GetComponent<RectTransform>();
		}

		private void OnValidate()
		{
			UpdateSpacing();
		}

		private void Update()
		{
			int currentChildCount = transform.childCount;
			if (currentChildCount != previousChildCount)
			{
				UpdateSpacing();
				previousChildCount = currentChildCount;
			}
		}


		public void UpdateSpacing()
		{
			float offset = 0;

			for (int i = 0; i < transform.childCount; i++)
			{
				RectTransform rect = transform.GetChild(i).GetComponent<RectTransform>();

				Vector2 anchor = GetAnchor(horizontalAllignment, verticalAlligment);

				float update;
				UpdateObject(out update, offset, anchor, rect);

				offset += update + Spacing;
			}

			rect.sizeDelta = new Vector2(rect.sizeDelta.x, -offset);
		}

		private Vector2 GetAnchor(HorizontalAllignment hor, VerticalAllignment ver)
		{
			float x = 0;
			switch (hor)
			{
				case HorizontalAllignment.Left:
					x = -(rect.sizeDelta.x / 2);
					break;
				case HorizontalAllignment.Centre:
					x = 0;
					break;
				case HorizontalAllignment.Right:
					x = (rect.sizeDelta.x / 2);
					break;
			}

			float y = 0;
			switch (ver)
			{
				case VerticalAllignment.Top:
					y = (rect.sizeDelta.y / 2);
					break;
				case VerticalAllignment.Centre:
					y = 0;
					break;
				case VerticalAllignment.Bottom:
					y = -(rect.sizeDelta.y / 2);
					break;
			}

			x += rect.position.x;
			y += rect.position.y;

			return new Vector2(x, y);
		}


		public abstract void UpdateObject(out float offSetUpdate, float offSet, Vector2 anchor, RectTransform rect);
	}
}
