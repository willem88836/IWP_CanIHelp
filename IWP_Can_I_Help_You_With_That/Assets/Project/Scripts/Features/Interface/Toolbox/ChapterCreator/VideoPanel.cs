using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterface
{
	public class VideoPanel : MonoBehaviour
	{
		public Button SelectButton;
		public Text NamePanel;
		public Image Logo;
		public Color SelectedColor;
		public Color UnSelectedColor;

		private string fileName;
		private ChapterCreator creator;


		public void Initialize(ChapterCreator creator, string fileName)
		{
			this.creator = creator;
			this.fileName = fileName;
			NamePanel.text = Path.GetFileNameWithoutExtension(fileName);
			SelectButton.onClick.AddListener(OnClick);


			// TODO: set something like a thumbnail as logo. 
			Deselect();
		}

		private void OnClick()
		{
			creator.OnPathSelected(fileName);
		}

		internal void Select()
		{
			Logo.color = SelectedColor;
		}

		internal void Deselect()
		{
			Logo.color = UnSelectedColor;
		}
	}
}
