using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.Explorer
{
	public class ExplorerViewObject : MonoBehaviour
	{
		public Text Label;
		public Button Button;

		public bool IsFile { get { return File.Exists(Path); } }
		public ExplorerView Parent { get; private set; }
		public string Path { get; private set; }


		private void Awake()
		{
			Button.onClick.AddListener(OnClick);
		}

		public void Initialize(ExplorerView parent, string path)
		{
			this.Parent = parent;
			this.Path = path;

			string name = "";

			if (File.Exists(path))
				System.IO.Path.GetFileNameWithoutExtension(path);
			else if (Directory.Exists(path))
				System.IO.Path.GetDirectoryName(path);

			Label.text = name;
		}

		public void OnClick()
		{
			Parent.OnClick(this);
		}
	}
}
