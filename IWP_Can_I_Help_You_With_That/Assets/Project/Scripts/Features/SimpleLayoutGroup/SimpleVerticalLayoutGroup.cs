using UnityEngine;

namespace IWPCIH.LayoutGroup
{
	public sealed class SimpleVerticalLayoutGroup : SimpleLayoutGroup
	{
		protected override void UpdateLayoutGroup()
		{
			float height = Rect.position.y;

			for (int i = 0; i < ChildCount; i++)
			{
				RectTransform rect = transform.GetChild(i).GetComponent<RectTransform>();
				if (!rect)
					continue;

				SetAnchors(rect);
				rect.position = new Vector3(rect.position.x, height, rect.position.z);

				height -= rect.sizeDelta.y + Spacing;
			}

			height -= Rect.position.y;

			if (FitChildSize)
			{
				RectTransform rect = Rect;
				rect.sizeDelta = new Vector2(rect.sizeDelta.x, -height);
			}
		}
	}
}
