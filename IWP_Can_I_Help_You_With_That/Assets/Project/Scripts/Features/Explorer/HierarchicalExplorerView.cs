using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.Explorer
{
	public class HierarchicalExplorerView : ExplorerView
	{
		public float IndentWidth;

		private List<ExplorerViewObject> evos = new List<ExplorerViewObject>();
		private float maxIndenting;


		public override void Initialize(string path)
		{
			evos.Clear();
			Clear();

			string[] pathSegments = path.Split('/', '\\');

			int ddi = 0; // Derived Directory Index.
			string currentDir = "";
			for (int i = 0; i < pathSegments.Length - 1; i++)
			{
				string currentSegment = pathSegments[i];
				currentDir += currentSegment + '\\';
				List<ExplorerViewObject> createdFolders = CreateFolders(currentDir, ContentContainer, ApplyIndenting);

				Insert(createdFolders, ddi);
				ddi += createdFolders.FindIndex((ExplorerViewObject ov) => ov.Path == currentDir + pathSegments[i + 1]);
			}

			List<ExplorerViewObject> createdFiles = CreateFiles(path, ContentContainer, ApplyIndenting);
			Insert(createdFiles, ddi);

			SortHierarchy();
			return;// TODO: Continue here! -> update the width of the container based on children.
			RectTransform rect = ContentContainer.GetComponent<RectTransform>();
			rect.sizeDelta = new Vector2(
				FileObject.GetComponent<RectTransform>().sizeDelta.x + maxIndenting, 
				rect.sizeDelta.y);
		}

		private void ApplyIndenting(ExplorerViewObject evo)
		{
			float indenting = (evo.Path.Split('\\', '/').Length - 2) * IndentWidth;
			RectTransform rect = evo.GetComponent<RectTransform>();
			
			rect.position = new Vector2(rect.position.x + indenting, rect.position.y);
			
			if (indenting > maxIndenting)
			{
				maxIndenting = indenting;
			}
		}

		private void Insert(List<ExplorerViewObject> objects, int baseIndex)
		{
			for (int j = 0; j < objects.Count; j++)
			{
				evos.Insert(j + baseIndex, objects[j]);
			}
		}

		private void SortHierarchy()
		{
			for (int i = 0; i < evos.Count; i++)
			{
				ExplorerViewObject evo = evos[i];
				evo.transform.SetSiblingIndex(i);
			}
		}
	}
}
