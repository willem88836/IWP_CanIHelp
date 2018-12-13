using UnityEngine;

namespace IWPCIH.Explorer
{
	public class ExplorerFile3D : ExplorerObject
	{
		public TextMesh Text;
		public bool ShowExtention;
		public int TextWidth = 8;
		public int MaxRows = 3;

		public override void Initialize(ExplorerView explorerView, string path)
		{
			base.Initialize(explorerView, path);
			string text = ShowExtention
				? System.IO.Path.GetFileName(path)
				: System.IO.Path.GetFileNameWithoutExtension(path);

			text = WrapText(text);
			Text.text = text;
		}

		private string WrapText(string text)
		{
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
