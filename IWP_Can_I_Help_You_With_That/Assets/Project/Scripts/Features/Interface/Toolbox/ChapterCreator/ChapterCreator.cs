using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace IWPCIH.EditorInterface
{
	/// <summary>
	///		Class responsible for the creation of new chapters.
	/// </summary>
	public class ChapterCreator : MonoBehaviour
	{
		// Chapters that contain these icons cannot be created.
		// It will probably break something.
		public readonly char[] CUSTOMBLOCKEDCHARS = new char[] { 'Ϩ' };

		public InputField NameTextfield;
		public Explorer.Explorer VideoExplorer;
		public VideoPanel VideoPanelPrefab;
		public Transform VideoPanelContainer;


		private Dictionary<string, VideoPanel> usedVideos = new Dictionary<string, VideoPanel>();
		private string selectedPath = "";


		/// <summary>
		///		Used to connect with the explorer.
		/// </summary>
		private void OnEnable()
		{
			VideoExplorer.OnPathSelected = null;
			VideoExplorer.OnPathSelected += OnPathSelected;
		}
		private void OnDisable()
		{
			VideoExplorer.OnPathSelected -= OnPathSelected;
		}


		/// <summary>
		///		If all requirements are met,
		///		a new chapter is created. 
		/// </summary>
		public void Create()
		{
			string name = NameTextfield.text;

			if (IsValid(name) && File.Exists(selectedPath))
			{
				(TimelineController.Instance as TimelineEditor).AddChapter(name, selectedPath);
				usedVideos[selectedPath].Deselect();
				NameTextfield.text = "";
				selectedPath = "";
			}
			else
			{
				OnInvalidName();
			}
		}

		/// <summary>
		///		Tests the validity of the name.
		/// </summary>
		private bool IsValid(string name)
		{

			foreach (char c in name)
			{
				foreach (char bc in Path.GetInvalidFileNameChars())
					if (c == bc)
						return false;

				foreach (char cbc in CUSTOMBLOCKEDCHARS)
					if (c == cbc)
						return false;
			}

			return true;
		}

		/// <summary>
		///		Is called when the name is concidered incorrect.
		/// </summary>
		private void OnInvalidName()
		{
			// TODO: implement events that happen on an invalid name.
			throw new NotImplementedException();
		}

		/// <summary>
		///		Is called once a video path is selected.
		/// </summary>
		public void OnPathSelected(string path)
		{
			if (!File.Exists(path))
				return;

			VideoExplorer.gameObject.SetActive(false);

			// if a panel doesn't exists for this path, 
			// a new one is created and stored. 
			if (!usedVideos.ContainsKey(path))
			{
				VideoPanel newPanel = Instantiate(VideoPanelPrefab, VideoPanelContainer);
				usedVideos.Add(path, newPanel);
				newPanel.Initialize(this, path);
				newPanel.Select();

				LayoutRebuilder.ForceRebuildLayoutImmediate(VideoPanelContainer.GetComponent<RectTransform>());
			}
			
			// Updated selection.
			if (usedVideos.ContainsKey(selectedPath))
				usedVideos[selectedPath].Deselect();
			usedVideos[path].Select();
			selectedPath = path;
		}
	}
}
