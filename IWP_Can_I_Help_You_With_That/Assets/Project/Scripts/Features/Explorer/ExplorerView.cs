using Framework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		public string[] ShownExtentions;
		public Transform ContentContainer;

		public string Path { get; protected set; }

		protected Explorer parent; 


		public virtual void Initialize(Explorer parent, string path)
		{
			this.parent = parent;
			Path = path + '\\';

			Clear();
			CreateFolders(Path, ContentContainer);
			CreateFiles(Path, ContentContainer);
		}

		protected void Clear()
		{
			for (int i = ContentContainer.childCount - 1; i >= 0; i--)
			{
				Destroy(ContentContainer.GetChild(i).gameObject);
			}
		}

		protected List<ExplorerViewObject> CreateFolders(
			string path, 
			Transform container, 
			Action<ExplorerViewObject> onInitialize = null)
		{
			List<ExplorerViewObject> objects = new List<ExplorerViewObject>();

			if (FolderObject)
			{
				Utilities.ForeachFolderAt(path, (string p) =>
				{
					if (HasBlockedAttributes(p))
						return;

					ExplorerViewObject newObj = Instantiate(FolderObject, container);
					newObj.Initialize(this, p);
					newObj.gameObject.name = string.Format("Folder_({0})", p);
					objects.Add(newObj);
					onInitialize.SafeInvoke(newObj);
				});
			}

			return objects;
		}

		protected List<ExplorerViewObject> CreateFiles(
			string path, 
			Transform container, 
			Action<ExplorerViewObject> onInitialize = null)
		{
			List<ExplorerViewObject> objects = new List<ExplorerViewObject>();
			
			if (FileObject)
			{
				Utilities.ForeachFileAt(path, (FileInfo info) =>
				{
					if (HasBlockedAttributes(info.FullName) || HasBlockedExtention(info.FullName))
						return;

					ExplorerViewObject newObj = Instantiate(FileObject, container);
					newObj.Initialize(this, info.FullName);
					newObj.gameObject.name = string.Format("File_({0})", info.FullName);
					objects.Add(newObj);
					onInitialize.SafeInvoke(newObj);
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

		private bool HasBlockedExtention(string path)
		{
			string extention = System.IO.Path.GetExtension(path);
			return ShownExtentions.Length == 0 
				? false
				: !ShownExtentions.Contains(extention);
		}

		public void OnClick(ExplorerViewObject invoked)
		{
			if (!parent)
				return;

			Type t = invoked.GetType();
			if (t == typeof(ExplorerViewObjectFile))
				parent.OnFileInvoke(this, invoked);
			else if (t == typeof(ExplorerViewObjectFolder))
				parent.OnFolderInvoke(this, invoked);
		}
	}
}
