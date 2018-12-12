using UnityEngine;

namespace IWPCIH.ExplorerII
{
	public abstract class ExplorerObject : MonoBehaviour
	{
		public string Path { get; protected set; }

		protected ExplorerView ParentView;

		public virtual void Initialize(ExplorerView explorerView, string path)
		{
			ParentView = explorerView;
			Path = path;
		}

		protected virtual void OnSelect()
		{
			ParentView.OnSelect(this);
		}
	}
}
