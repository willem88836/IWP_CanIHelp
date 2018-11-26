using System.IO;

namespace IWPCIH.Explorer
{
	public class ExplorerViewObjectFile : ExplorerViewObject
	{
		public bool ShowExtention;

		public override void Initialize(ExplorerView parentView, string path)
		{
			base.Initialize(parentView, path);

			string name = ShowExtention
				? Path.GetFileName(path)
				: Path.GetFileNameWithoutExtension(path);

			Label.text = name;
		}
	}
}
