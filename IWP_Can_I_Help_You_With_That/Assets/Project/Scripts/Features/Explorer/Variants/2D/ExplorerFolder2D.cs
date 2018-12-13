using System.IO;
using UnityEngine.UI;

namespace IWPCIH.ExplorerII
{
	public class ExplorerFolder2D : ExplorerObject
	{
		public Text Label;
		public Button Button;

		public override void Initialize(ExplorerView explorerView, string path)
		{
			base.Initialize(explorerView, path);

			Label.text = new DirectoryInfo(path).Name;
			Button.onClick.AddListener(OnSelect);
		}
	}
}
