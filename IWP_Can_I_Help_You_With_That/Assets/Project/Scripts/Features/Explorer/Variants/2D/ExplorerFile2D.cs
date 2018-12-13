using UnityEngine.UI;

namespace IWPCIH.ExplorerII
{
	public class ExplorerFile2D : ExplorerObject
	{
		public Text Label;
		public Button Button;
		public bool ShowExtention;

		public override void Initialize(ExplorerView explorerView, string path)
		{
			base.Initialize(explorerView, path);

			string name = ShowExtention
				? System.IO.Path.GetFileName(path)
				: System.IO.Path.GetFileNameWithoutExtension(path);

			Label.text = name;

			Button.onClick.AddListener(OnSelect);
		}
	}
}
