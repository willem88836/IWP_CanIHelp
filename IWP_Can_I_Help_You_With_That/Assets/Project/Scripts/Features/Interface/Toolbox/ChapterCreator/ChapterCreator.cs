using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IWPCIH.Explorer;
using System.IO;

namespace IWPCIH.EditorInterface
{
	public class ChapterCreator : MonoBehaviour
	{
		public readonly char[] CUSTOMBLOCKEDCHARS = new char[] { 'Ϩ' };


		public InputField NameTextfield;
		public Explorer.Explorer VideoExplorer;
		public VideoPanel VideoPanelPrefab;
		public Transform VideoPanelContainer;



		private Dictionary<string, VideoPanel> usedVideos = new Dictionary<string, VideoPanel>();


		private string selectedPath = "";


		private void OnEnable()
		{
			VideoExplorer.OnPathSelected = null;
			VideoExplorer.OnPathSelected += OnPathSelected;
		}
		private void OnDisable()
		{
			VideoExplorer.OnPathSelected -= OnPathSelected;
		}



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

		private void OnInvalidName()
		{
			// TODO: implement events that happen on an invalid name.
			throw new NotImplementedException();
		}

		
		public void OnPathSelected(string path)
		{
			if (!File.Exists(path))
				return;

			VideoExplorer.gameObject.SetActive(false);

			if (!usedVideos.ContainsKey(path))
			{
				VideoPanel newPanel = Instantiate(VideoPanelPrefab, VideoPanelContainer);
				usedVideos.Add(path, newPanel);
				newPanel.Initialize(this, path);
				newPanel.Select();

				LayoutRebuilder.ForceRebuildLayoutImmediate(VideoPanelContainer.GetComponent<RectTransform>());
			}

			if (usedVideos.ContainsKey(selectedPath))
				usedVideos[selectedPath].Deselect();
			usedVideos[path].Select();
			selectedPath = path;
		}
	}
}
