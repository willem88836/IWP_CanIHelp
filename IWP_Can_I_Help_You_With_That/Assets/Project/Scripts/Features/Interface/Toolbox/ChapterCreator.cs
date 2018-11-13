using System;
using UnityEngine;
using UnityEngine.UI;


namespace IWPCIH.EditorInterface
{
	public class ChapterCreator : MonoBehaviour
	{
		public Text NameTextfield;


		public void Create()
		{
			string name = NameTextfield.text;

			if (IsValid(name))
			{
				TimelineController.instance.AddChapter(NameTextfield.text, "VideoName"); // TODO: make this the actual video name. 
			}
			else
			{
				OnInvalidName();
			}
		}

		private bool IsValid(string name)
		{
			// TODO: create an actual validity check.
			return true;
		}

		private void OnInvalidName()
		{
			// TODO: implement events that happen on an invalid name.
			throw new NotImplementedException();
		}
	}
}
