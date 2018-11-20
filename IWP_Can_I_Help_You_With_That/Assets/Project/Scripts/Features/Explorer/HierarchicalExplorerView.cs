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
		private List<ExplorerViewObject> evos = new List<ExplorerViewObject>();

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
				List<ExplorerViewObject> createdFolders = CreateFolders(currentDir, ContentContainer);

				Insert(createdFolders, ddi);
				ddi += createdFolders.FindIndex((ExplorerViewObject ov) => ov.Path == currentDir + pathSegments[i + 1]);
			}

			List<ExplorerViewObject> createdFiles = CreateFiles(path, ContentContainer);
			Insert(createdFiles, ddi);

			SortHierarchy();
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
