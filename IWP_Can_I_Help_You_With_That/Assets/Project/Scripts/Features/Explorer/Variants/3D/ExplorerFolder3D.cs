using UnityEngine;

namespace IWPCIH.Explorer
{
	public sealed class ExplorerFolder3D : ExplorerObject3D
	{
		public TextMesh Text;

		public override void Initialize(ExplorerView explorerView, string path)
		{
			base.Initialize(explorerView, path);
			// TODO: Check if this is the proper way to get the folder name..
			// TODO: make this an ExplorerObject thing.
			string[] splittedPath = path.Split('\\', '/');
			string text = splittedPath[splittedPath.Length - ((path.EndsWith("\\") || path.EndsWith("/")) ? 2 : 1)];
			text = WrapText(text);
			Text.text = text;
		}

		public override void Select()
		{
			OnSelect();
		}
	}
}
