using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterface
{
	public class VideoPanel : MonoBehaviour
	{
		public Text NamePanel;
		public Image Logo;

		public void Initialize(string fileName)
		{
			// TODO: set something like a thumbnail as logo. 
		}

		internal void Select()
		{
			Debug.Log("Selected video panel!");
		}

		internal void Deselect()
		{
			Debug.Log("Deselected video panel!");
		}
	}
}
