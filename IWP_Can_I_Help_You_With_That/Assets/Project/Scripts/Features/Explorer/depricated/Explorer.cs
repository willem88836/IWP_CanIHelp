using Framework.Core;
using Framework.ScriptableObjects.Variables;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.Explorer
{
	// TODO: make this not 2D only. Make this abstract and let 3D and 2D variants derive from this. 
	/// <summary>
	///		Responsible for updating and controlling
	///		multiple ExplorerViews.
	/// </summary>
	public class Explorer : MonoBehaviour
	{
		[Serializable]
		public class ExplorerViewUpdate
		{
			public enum UpdateType { None, All, OnlySelf, AllExceptSelf }

			public ExplorerView View;
			public UpdateType Type = UpdateType.None;
		}


		public Action<string> OnPathSelected;

		public StringReference ExplorerPath;
		public List<ExplorerViewUpdate> Actions;

		[Space]
		public Text SelectedPath;
		public Button SelectedPathConfirmationButton;
		public InputField PathInputField;
		public Button PathInputFieldConfirmationButton;

		private string selectedPath = "";


		public void Start()
		{
			SelectedPathConfirmationButton.onClick.AddListener(OnSelectedPathConfirmed);
			PathInputFieldConfirmationButton.onClick.AddListener(delegate 
			{
				if(Directory.Exists(PathInputField.text))
					UpdateAllViews(PathInputField.text);
			});
			UpdateAllViews(ExplorerPath.Value);
		}

		
		public void OnSelectedPathConfirmed()
		{
			if (File.Exists(selectedPath) || Directory.Exists(selectedPath))
				OnPathSelected.SafeInvoke(selectedPath);
		}
		public void Back()
		{
			string dirName = Path.GetDirectoryName(ExplorerPath.Value);

			if (dirName == null)
				return;

			if (dirName.EndsWith("\\"))
				dirName = dirName.TrimEnd('\\');

			if (Directory.Exists(dirName))
			{
				ExplorerPath.Value = dirName;
				UpdateAllViews(ExplorerPath.Value);
			}
		}

		private void SelectPath(string path)
		{
			ExplorerPath.Value = path;
			if (Directory.Exists(path))
				PathInputField.text = path;
			selectedPath = path;
			SelectedPath.text = selectedPath;
		}

		/// <summary>
		///		Is called once a file is selected.
		/// </summary>
		public void OnFileInvoke(ExplorerView parent, ExplorerViewObject invoked)
		{
			SelectPath(invoked.MyPath);
		}
		/// <summary>
		///		Is called once a folder is selected.
		/// </summary>
		public void OnFolderInvoke(ExplorerView parent, ExplorerViewObject invoked)
		{
			ExplorerViewUpdate va = Actions.Find((ExplorerViewUpdate v) => v.View == parent);

			if (va != null)
				UpdateViews(va, invoked.MyPath);

			SelectPath(invoked.MyPath);
			PathInputField.text = invoked.MyPath;
		}


		/// <summary>
		///		Updates all views to the provided path.
		/// </summary>
		private void UpdateAllViews(string path)
		{
			SelectPath(path);
			foreach (ExplorerViewUpdate action in Actions)
			{
				UpdateView(action, path);
			}
		}
		/// <summary>
		///		Updates all views relative to a the ViewAction's UpdateType.
		/// </summary>
		private void UpdateViews(ExplorerViewUpdate evu, string path)
		{
			SelectPath(path);

			if (evu.Type == ExplorerViewUpdate.UpdateType.None)
				return;

			if (evu.Type == ExplorerViewUpdate.UpdateType.OnlySelf)
			{
				UpdateView(evu, path);
				return;
			}

			foreach (ExplorerViewUpdate action in Actions)
			{
				if (evu.Type == ExplorerViewUpdate.UpdateType.AllExceptSelf && action == evu)
					continue;

				UpdateView(action, path);
			}
		}
		/// <summary>
		///		Updates one specific ViewAction's ExplorerView.
		/// </summary>
		private void UpdateView(ExplorerViewUpdate evu, string path)
		{
			evu.View.Initialize(this, path);
		}
	}
}
