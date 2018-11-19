using System;
using System.IO;
using UnityEngine;

namespace IWPCIH.Explorer
{
	public class ExplorerView : MonoBehaviour
	{
		public ExplorerViewObject FolderObject;
		public ExplorerViewObject FileObject;
		public Transform ContentContainer;

		public string Path { get; private set; }

		public Action<ExplorerView, ExplorerViewObject> OnObjectInvoke;


		internal void Initialize(string path)
		{
			this.Path = path;

			Clear();
			CreateFolders(path);
			CreateFiles(path);
		}

		private void Clear()
		{
			for (int i = ContentContainer.childCount - 1; i >= 0; i++)
			{
				Destroy(ContentContainer.GetChild(i).gameObject);
			}
		}

		private void CreateFolders(string path)
		{
			if (FolderObject)
			{
				Utilities.ForeachFolderAt(path, (string p) =>
				{
					ExplorerViewObject newObj = Instantiate(FolderObject, ContentContainer);
					newObj.Initialize(this, p);
				});
			}
		}

		private void CreateFiles(string path)
		{
			if (FileObject)
			{
				Utilities.ForeachFileAt(path, (FileInfo info) =>
				{
					ExplorerViewObject newObj = Instantiate(FileObject, ContentContainer);
					newObj.Initialize(this, info.FullName);
				});
			}
		}

		public void OnClick(ExplorerViewObject invoked)
		{
			OnObjectInvoke.SafeInvoke(this, invoked);
		}
	}
}
