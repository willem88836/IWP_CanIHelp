using UnityEngine;

namespace IWPCIH.LayoutGroup
{
	public sealed class SimpleHorizontalLayoutGroup : SimpleLayoutGroup
	{
		protected override void UpdateLayoutGroup()
		{
			float width = Rect.position.x;

			for (int i = 0; i < ChildCount; i++)
			{
				RectTransform rect = transform.GetChild(i).GetComponent<RectTransform>();
				if (!rect)
					continue;

				SetAnchors(rect);
				rect.position = new Vector3(width, rect.position.y, rect.position.z);

				width -= rect.sizeDelta.x + Spacing;
			}

			width -= Rect.position.x;

			if (FitChildSize)
			{
				RectTransform rect = Rect;
				rect.sizeDelta = new Vector2(-width, rect.sizeDelta.y);
			}
		}
	}
}
