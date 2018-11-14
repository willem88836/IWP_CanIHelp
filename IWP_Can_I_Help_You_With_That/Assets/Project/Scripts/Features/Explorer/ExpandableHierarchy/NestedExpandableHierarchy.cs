using UnityEngine;

namespace IWPCIH.Explorer
{
	public class NestedExpandableHierarchy : MonoBehaviour
	{
		public HierarchyObject BaseObject;
		public Transform Body;
		public float minHeight = 50;
		public float nestedOffSet = 10;


		public void Create(HierarchyObjectData hierarchy)
		{
			Transform parent = Instantiate(BaseObject, Body).Body;
			ForeachData(hierarchy, parent);
		}

		private void ForeachData(HierarchyObjectData data, Transform parent)
		{
			HierarchyObject obj = Instantiate(BaseObject, parent);
			obj.Header.text = data.Text;

			foreach (HierarchyObjectData hier in data.Content)
			{
				ForeachData(hier, obj.Body);
			}

			RectTransform rect = obj.GetComponent<RectTransform>();
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, data.Content.Length);
		}
	}
}
