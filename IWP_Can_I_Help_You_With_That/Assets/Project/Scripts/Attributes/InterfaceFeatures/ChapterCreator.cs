using System;
using UnityEngine;
using UnityEngine.UI;


namespace IWPCIH.InterfaceFeatures
{
	public class ChapterCreator : MonoBehaviour
	{
		public Text NameTextfield;


		public void Create()
		{
			string name = NameTextfield.text;

			if (IsValid(name))
			{
				TimelineController.instance.AddChapter(NameTextfield.text);
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
			// TODO: implement this stuff. .
			throw new NotImplementedException();
		}
	}
}
