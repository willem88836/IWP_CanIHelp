using UnityEngine;

namespace IWPCIH.Explorer
{
	public sealed class SimpleHorizontalLayoutGroup : SimpleLayoutGroup
	{
		public override void UpdateObject(out float offSetUpdate, float offSet, Vector2 anchor, RectTransform rect)
		{
			rect.position = new Vector3(offSet + anchor.x, rect.position.z, rect.position.z);
			offSetUpdate = rect.sizeDelta.x;
		}
	}
}

