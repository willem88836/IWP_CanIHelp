using UnityEngine;

namespace IWPCIH.Explorer
{
	public class ExplorerFolder3D : ExplorerObject, ISelectable
	{
		public TextMesh Text;
		public int TextWidth = 8;
		public int MaxRows = 3;

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

		public void Select()
		{
			OnSelect();
		}

		private string WrapText(string text)
		{
			// TODO: Improve this stuff. this cuts words in half.
			// TODO: Make this accessible for all 3D texts instead of in each ExplorerObject.
			// This stuff goes for ExplorerFile3D as well.
			int loops = Mathf.FloorToInt(text.Length / TextWidth);
			loops = Mathf.Min(loops, MaxRows);

			for (int i = TextWidth; i < text.Length; i += TextWidth)
			{
				if (i % TextWidth >= loops)
					break;

				int index = i + i / TextWidth;
				if (index < text.Length)
					text = text.Insert(index, "\n");
			}

			return text;
		}
	}
}
