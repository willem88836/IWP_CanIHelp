using Framework.Core;
using Framework.ScriptableObjects.Variables;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IWPCIH.Explorer
{
	/// <summary>
	///		Contains functionality to control and maintain 
	///		multiple explorer views.
	/// </summary>
	public class Explorer : MonoBehaviour
	{
		public StringReference ExplorerPath;
		public List<ExplorerView> Views;

		public Action<string> OnPathSelected;


		public virtual void OnEnable()
		{
			UpdateAllViews(ExplorerPath.Value);
		}


		/// <summary>
		///		Invokes OnPathSelected with the current path. 
		/// </summary>
		public virtual void ConfirmSelectedPath()
		{
			string path = ExplorerPath.Value;
			if (File.Exists(path) || Directory.Exists(path))
				OnPathSelected.SafeInvoke(path);
		}
		/// <summary>
		///		Updates all views to the parent directory
		///		of the current path.
		/// </summary>
		public virtual void Back()
		{
			string dirName = "";
			try
			{
				dirName = Path.GetDirectoryName(ExplorerPath.Value);
			} catch (Exception ex) { };

			if (dirName == null)
				return;

			if (dirName.EndsWith("\\"))
				dirName = dirName.TrimEnd('\\');

			ExplorerPath.Value = dirName;
			UpdateAllViews(dirName);
		}


		/// <summary>
		///		Is called once a ViewObject is selected.
		/// </summary>
		public virtual void OnObjectSelected(string path)
		{
			if (File.Exists(path))
			{
				path = Path.GetDirectoryName(path);
			}
			else if(!Directory.Exists(path))
			{
				throw new FileNotFoundException("Selected path does not exist!");
			}

			UpdateAllViews(path);
		}


		/// <summary>
		///		Updates all views to the provided path.
		/// </summary>
		protected virtual void UpdateAllViews(string path)
		{
			// Prevents the explorer from attempting 
			// to load a file instead of a directory.
			if (File.Exists(path))
			{
				path = Path.GetDirectoryName(path);
			}

			if (!Directory.Exists(path))
				Debug.LogWarning("Updating views with non-existing path!");

			foreach (ExplorerView view in Views)
			{
				UpdateView(view, path);
			}
		}
		/// <summary>
		///		Updates one specific ViewAction's ExplorerView.
		/// </summary>
		protected virtual void UpdateView(ExplorerView view, string path)
		{
			view.Initialize(this, path);
		}
	}
}
