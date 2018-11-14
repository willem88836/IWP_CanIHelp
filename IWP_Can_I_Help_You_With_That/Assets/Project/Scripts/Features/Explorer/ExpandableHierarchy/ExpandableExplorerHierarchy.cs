using System.IO;
using UnityEngine;

namespace IWPCIH.Explorer
{
	public class ExpandableExplorerHierarchy : MonoBehaviour
	{
		public HierarchyObject BaseObject;	
		public Transform Body;


		private void Start()
		{
			CreateHierarchy("C:/Users/wille");
		}

		public void CreateHierarchy(params string[] startPaths)
		{
			HierarchyObjectData hierarchyData = new HierarchyObjectData()
			{
				Content = new HierarchyObjectData[startPaths.Length]
			};

			for (int i = 0; i < startPaths.Length; i++)
			{
				string current = startPaths[i];

				Transform previousParent = Body;
				Transform currentParent = Body;

				Utilities.ForeachFolderIn(
					current, 
					(string path) =>
					{
						DirectoryInfo info = new DirectoryInfo(path);
						if (info.Attributes == FileAttributes.Hidden || info.Attributes == FileAttributes.System)
							return;

						HierarchyObject newObj = Instantiate(BaseObject, Body);
						newObj.Header.text = Path.GetFileNameWithoutExtension(path);
						previousParent = currentParent;
						currentParent = newObj.Body;
					},
					(string path) =>
					{
						currentParent = previousParent;
					}
				);
			}
		}
	}
}
