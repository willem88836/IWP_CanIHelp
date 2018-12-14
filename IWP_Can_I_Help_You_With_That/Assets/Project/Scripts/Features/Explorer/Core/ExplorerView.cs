using Framework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace IWPCIH.Explorer
{
	/// <summary>
	///		Controls one view in which the contents of one 
	///		specific directory location are shown.
	/// </summary>
	public class ExplorerView : MonoBehaviour
	{
		public ExplorerObject FolderObject;
		public ExplorerObject FileObject;
		public Transform ContentContainer;

		[Space]
		public FileAttributes[] BlockedFileAttributes = new FileAttributes[]
		{
			FileAttributes.Hidden,
			FileAttributes.System
		};
		public string[] ShownExtentions;

		protected Explorer Explorer;


		/// <summary>
		///		Creates all objects at the provide path.
		/// </summary>
		public virtual void Initialize(Explorer explorer, string path)
		{
			Explorer = explorer;

			Clear();

			if (Directory.Exists(path))
			{
				CreateFolders(path, ContentContainer);
				CreateFiles(path, ContentContainer);
			}
			else
			{
				CreateDrives(ContentContainer);
			}
		}


		/// <summary>
		///		Removes all current objects.
		/// </summary>
		protected virtual void Clear()
		{
			for (int i = ContentContainer.childCount - 1; i >= 0; i--)
			{
				Destroy(ContentContainer.GetChild(i).gameObject);
			}
		}


		/// <summary>
		///		Creates a new instance of the provided object.
		/// </summary>
		protected virtual ExplorerObject CreateObject(
			string path,
			ExplorerObject explorerObject,
			Transform container,
			Action<ExplorerObject> onInitialize, 
			bool evaluatePath = true)
		{
			if (evaluatePath && (HasBlockedAttributes(path) || HasBlockedExtention(path)))
				return null;

			ExplorerObject newObj = Instantiate(explorerObject, container);
			newObj.Initialize(this, path);
			newObj.gameObject.name = string.Format("({0})_({1})", explorerObject.GetType().ToString(), path);
			onInitialize.SafeInvoke(newObj);

			return newObj;
		}

		/// <summary>
		///		Creates all ready drives as folder objects
		///		if the folder prefab is set.
		/// </summary>
		protected virtual List<ExplorerObject> CreateDrives( 
			Transform container,
			Action<ExplorerObject> onInitialize = null)
		{
			if (FolderObject)
			{
				List<ExplorerObject> objects = new List<ExplorerObject>();

				string[] drives = Directory.GetLogicalDrives();

				foreach (string d in drives)
				{
					if (!new DriveInfo(d).IsReady)
						continue;

					ExplorerObject newObject = CreateObject(d, FolderObject, ContentContainer, onInitialize, false);
					if (newObject != null)
						objects.Add(newObject);
				}

				return objects;
			}

			return null;
		}

		/// <summary>
		///		Creates all folder objects
		///		if a folder prefab is set.
		/// </summary>
		protected virtual List<ExplorerObject> CreateFolders(
			string path,
			Transform container,
			Action<ExplorerObject> onInitialize = null)
		{
			if (FolderObject)
			{
				List<ExplorerObject> objects = new List<ExplorerObject>();

				Utilities.ForeachFolderAt(path, (string p) =>
				{
					ExplorerObject explorerObject = CreateObject(p, FolderObject, ContentContainer, onInitialize);
					if (!explorerObject)
						objects.Add(explorerObject);
				});
				return objects;
			}

			return null;
		}

		/// <summary>
		///		Creates all file objects
		///		if a file prefab is set.
		/// </summary>
		protected virtual List<ExplorerObject> CreateFiles(
			string path,
			Transform container,
			Action<ExplorerObject> onInitialize = null)
		{
			if (FolderObject)
			{
				List<ExplorerObject> objects = new List<ExplorerObject>();

				Utilities.ForeachFolderAt(path, (string p) =>
				{
					ExplorerObject explorerObject = CreateObject(p, FileObject, ContentContainer, onInitialize);
					if (!explorerObject)
						objects.Add(explorerObject);
				});
				return objects;
			}

			return null;
		}

		/// <summary>
		///		Returns true if the path has blocked attributes.
		/// </summary>
		private bool HasBlockedAttributes(string path)
		{
			FileAttributes attributes = File.GetAttributes(path);

			foreach (FileAttributes blocked in BlockedFileAttributes)
			{
				if ((attributes & blocked) == blocked)
					return true;
			}

			return false;
		}

		/// <summary>
		///		Returns true if the path's extention is 
		///		not listed in 'ShownExtentions' . 
		/// </summary>
		private bool HasBlockedExtention(string path)
		{
			if (Directory.Exists(path))
				return false;

			string extention = Path.GetExtension(path);
			return ShownExtentions.Length == 0
				? false
				: !ShownExtentions.Contains(extention);
		}

		/// <summary>
		///		Calls OnObjectSelected in Explorer.
		/// </summary>
		public void OnSelect(ExplorerObject invoked)
		{
			if (Explorer)
				Explorer.OnObjectSelected(invoked.Path);
		}
	}
}
