using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterface
{
	/// <summary>
	///		Visual representation of one videopath.
	/// </summary>
	public class VideoPanel : MonoBehaviour
	{
		public Button SelectButton;
		public Text NamePanel;
		public Image Logo;
		public Color SelectedColor;
		public Color UnSelectedColor;

		private string fileName;
		private ChapterCreator creator;


		/// <summary>
		///		Updates the icon to it's default state
		///		relative to the provided parameters. 
		/// </summary>
		public void Initialize(ChapterCreator creator, string fileName)
		{
			this.creator = creator;
			this.fileName = fileName;
			NamePanel.text = Path.GetFileNameWithoutExtension(fileName);
			SelectButton.onClick.AddListener(OnClick);


			// TODO: set something like a thumbnail as logo. 
			Deselect();
		}

		/// <summary>
		///		Calls the chapter creator and sets
		///		this panel's path as the selected.
		/// </summary>
		private void OnClick()
		{
			creator.OnPathSelected(fileName);
		}

		/// <summary>
		///		Visual update for selection.
		/// </summary>
		internal void Select()
		{
			Logo.color = SelectedColor;
		}

		/// <summary>
		///		Visual Update for deselection.
		/// </summary>
		internal void Deselect()
		{
			Logo.color = UnSelectedColor;
		}
	}
}
