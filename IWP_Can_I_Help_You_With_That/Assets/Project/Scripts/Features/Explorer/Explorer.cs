using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IWPCIH.Explorer
{
	public class Explorer : MonoBehaviour
	{
		public ExplorerObject BaseObject;
		public Transform Body;
		public float indentWidth = 40;

		private List<ExplorerObject> hierarchyObjects = new List<ExplorerObject>();


		public void Expand(ExplorerObject expanded)
		{
			int i = expanded.Index;

			Utilities.ForeachFolderAt(expanded.Path, (string path) =>
			{
				ExplorerObject newObj = Instantiate(BaseObject, Body);
				newObj.HeaderText.text = Path.GetDirectoryName(path);
				newObj.Initialize(this);

				int depth = path.Split('\\', '/').Length;

				RectTransform rect = newObj.GetComponent<RectTransform>();
				rect.anchorMin = new Vector2(rect.position.x - (rect.sizeDelta.x / 2) + depth * indentWidth, rect.anchorMin.y);
				
				i++;
				hierarchyObjects.Insert(i, newObj);
			});

			UpdateIndex();
		}

		public void Shrink(ExplorerObject shrinked)
		{
			//for (int i = 0; i < shrinked.Body.childCount; i++)
			//{
			//	Transform child = shrinked.Body.GetChild(i);
			//	Destroy(child.gameObject);
			//}

			UpdateIndex();
		}

		private void UpdateIndex()
		{
			for (int i = 0; i < hierarchyObjects.Count; i++)
			{
				hierarchyObjects[i].Index = i;
			}
		}
	}
}
