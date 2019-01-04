using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IWPCIH.Explorer
{
	public sealed class HierarchicalExplorerView2D : ExplorerView
	{
		public float IndentWidth;

		private List<ExplorerObject> evos = new List<ExplorerObject>();
		private float maxIndenting;


		public override void Initialize(Explorer parent, string path)
		{
			evos.Clear();
			Clear();

			List<ExplorerObject> drives = CreateDrives(ContentContainer, ApplyIndenting);
			Insert(drives, 0);

			if (!Directory.Exists(path))
				return;

			List<string> pathSegments = new List<string>(path.Split('/', '\\'));
			string currentDir = pathSegments[0] + '\\'; // this is the drive.
			int insertIndex = drives.IndexOf(drives.Find((ExplorerObject eo) => eo.Path == currentDir)) + 1;
			for (int i = 1; i < pathSegments.Count; i++)
			{
				List<ExplorerObject> folders = CreateFolders(currentDir, ContentContainer, ApplyIndenting);
				if (folders != null)
					Insert(folders, insertIndex);

				currentDir = Path.Combine(currentDir, pathSegments[i]);
				insertIndex += folders.IndexOf(folders.Find((ExplorerObject eo) => eo.Path == currentDir)) + 1;
			}

			// TODO: Find a way to insert this into the loop.
			List<ExplorerObject> childFolders = CreateFolders(currentDir + '\\', ContentContainer, ApplyIndenting);
			if (childFolders != null)
				Insert(childFolders, insertIndex);

			// TODO: I'm not sure this works with the not updated insertIndex.
			List<ExplorerObject> files = CreateFiles(currentDir, ContentContainer, ApplyIndenting);
			if (files != null)
				Insert(files, insertIndex);

			SortHierarchy();
		}

		private void ApplyIndenting(ExplorerObject evo)
		{
			float indenting = (evo.Path.Split('\\', '/').Length - (evo.Path.EndsWith("\\") ? 2 : 1)) * IndentWidth;
			RectTransform rect = evo.GetComponent<RectTransform>();

			rect.position = new Vector2(rect.position.x + indenting / 2, rect.position.y);
			rect.sizeDelta = new Vector2(rect.sizeDelta.x - indenting, rect.sizeDelta.y);

			if (indenting > maxIndenting)
			{
				maxIndenting = indenting;
			}
		}

		private void Insert(List<ExplorerObject> objects, int baseIndex)
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
				evos[i].transform.SetSiblingIndex(i);
			}
		}
	}
}
