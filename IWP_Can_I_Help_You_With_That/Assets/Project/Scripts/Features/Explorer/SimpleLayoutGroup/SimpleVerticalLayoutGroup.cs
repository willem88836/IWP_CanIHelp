using UnityEngine;

namespace IWPCIH.Explorer
{
	public sealed class SimpleVerticalLayoutGroup : SimpleLayoutGroup
	{
		public override void UpdateObject(out float offSetUpdate, float offSet, Vector2 anchor, RectTransform rect)
		{
			rect.position = new Vector3(rect.position.x, offSet + anchor.y, rect.position.z);
			offSetUpdate = -rect.sizeDelta.y;
		}
	}
}

