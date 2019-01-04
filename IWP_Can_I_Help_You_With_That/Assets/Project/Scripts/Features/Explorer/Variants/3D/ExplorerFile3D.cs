using UnityEngine;

namespace IWPCIH.Explorer
{
	public sealed class ExplorerFile3D : ExplorerObject3D
	{
		public TextMesh Text;
		public bool ShowExtention;

		public override void Initialize(ExplorerView explorerView, string path)
		{
			base.Initialize(explorerView, path);
			string text = ShowExtention
				? System.IO.Path.GetFileName(path)
				: System.IO.Path.GetFileNameWithoutExtension(path);

			text = WrapText(text);
			Text.text = text;
		}

		public override void Select()
		{
			OnSelect();
		}
	}
}
