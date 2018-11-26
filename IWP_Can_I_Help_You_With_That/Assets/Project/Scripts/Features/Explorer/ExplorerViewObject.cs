using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.Explorer
{
	public abstract class ExplorerViewObject : MonoBehaviour
	{
		public Text Label;
		public Button Button;

		public ExplorerView ParentView { get; private set; }
		public string MyPath { get; private set; }


		public virtual void Initialize(ExplorerView parentView, string path)
		{
			ParentView = parentView;
			MyPath = path;

			Button.onClick.AddListener(OnClick);
		}

		protected virtual void OnClick()
		{
			ParentView.OnClick(this);
		}
	}
}
