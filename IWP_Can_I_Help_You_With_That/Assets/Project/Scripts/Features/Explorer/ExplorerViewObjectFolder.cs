using System.IO;

namespace IWPCIH.Explorer
{
	public class ExplorerViewObjectFolder : ExplorerViewObject
	{
		public override void Initialize(ExplorerView parentView, string path)
		{
			base.Initialize(parentView, path);
			Label.text = new DirectoryInfo(path).Name;
		}
	}
}
