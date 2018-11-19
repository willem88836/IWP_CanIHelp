using System;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.Explorer
{
	public class ExplorerObject : MonoBehaviour
	{
		public Button ExpandButton;
		public Button ShrinkButton;
		public Text HeaderText;

		[HideInInspector] public string Path;
		[HideInInspector] public int Index;

		private Explorer explorer;

		private bool isExpanded = false;


		public void Initialize(Explorer explorer)
		{
			this.explorer = explorer;

			ShrinkButton.onClick.AddListener(Shrink);
			ExpandButton.onClick.AddListener(Expand);
		}


		private void Expand()
		{
			if (!isExpanded)
				explorer.Expand(this);
			isExpanded = true;
		}

		private void Shrink()
		{
			if (isExpanded)
				explorer.Shrink(this);
			isExpanded = false;
		}
	}
}
