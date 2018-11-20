using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IWPCIH.Explorer
{
	public class ExplorerView : MonoBehaviour
	{
		private readonly FileAttributes[] blockedFileAttributes = new FileAttributes[] 
		{
			FileAttributes.Hidden,
			FileAttributes.System
		};


		public ExplorerViewObject FolderObject;
		public ExplorerViewObject FileObject;
		public Transform ContentContainer;

		public string Path { get; protected set; }

		public Action<ExplorerView, ExplorerViewObject> OnObjectInvoke;


		public virtual void Initialize(string path)
		{
			Path = path;

			Clear();
			CreateFolders(Path, ContentContainer);
			CreateFiles(Path, ContentContainer);
		}

		protected void Clear()
		{
			for (int i = ContentContainer.childCount - 1; i >= 0; i++)
			{
				Destroy(ContentContainer.GetChild(i).gameObject);
			}
		}

		protected List<ExplorerViewObject> CreateFolders(string path, Transform container)
		{
			List<ExplorerViewObject> objects = null;

			if (FolderObject)
			{
				objects = new List<ExplorerViewObject>();

				Utilities.ForeachFolderAt(path, (string p) =>
				{
					if (HasBlockedAttributes(p))
						return;	
					
					ExplorerViewObject newObj = Instantiate(FolderObject, container);
					newObj.Initialize(this, p);
					newObj.gameObject.name = string.Format("Folder_{0}", p);
					objects.Add(newObj);
				});
			}

			return objects;
		}

		protected List<ExplorerViewObject> CreateFiles(string path, Transform container)
		{
			List<ExplorerViewObject> objects = null;
			Debug.Log(path);
			if (FileObject)
			{
				objects = new List<ExplorerViewObject>(); 
				Utilities.ForeachFileAt(path, (FileInfo info) =>
				{
					if (HasBlockedAttributes(info.FullName))
						return;

					ExplorerViewObject newObj = Instantiate(FileObject, container);
					newObj.Initialize(this, info.FullName);
					newObj.gameObject.name = string.Format("File_{0}", info.Name);
					objects.Add(newObj);
				});

			}

			return objects;
		}


		private bool HasBlockedAttributes(string path)
		{
			FileAttributes attributes = File.GetAttributes(path);

			foreach (FileAttributes blocked in blockedFileAttributes)
			{
				if ((attributes & blocked) == blocked)
					return true;
			}

			return false;
		}


		public void OnClick(ExplorerViewObject invoked)
		{
			OnObjectInvoke.SafeInvoke(this, invoked);
		}
	}
}
