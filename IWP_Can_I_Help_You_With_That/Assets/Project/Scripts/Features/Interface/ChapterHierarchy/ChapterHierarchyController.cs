using IWPCIH.EventTracking;
using System.Collections.Generic;
using UnityEngine;


namespace IWPCIH.EditorInterface
{
	public class ChapterHierarchyController : MonoBehaviour
	{
		public Transform container;
		public ChapterHierarchyButton baseButton;

		private List<ChapterHierarchyButton> buttons = new List<ChapterHierarchyButton>();

		public void Clear()
		{
			foreach(ChapterHierarchyButton button in buttons)
			{
				Destroy(button.gameObject);
			}

			buttons.Clear();
		}

		public void AddChapter(TimelineChapter chapter)
		{
			ChapterHierarchyButton newButton = Instantiate(baseButton, container);
			newButton.Initialize(this, chapter);
			buttons.Add(newButton);
		}

		public void RemoveChapter(ChapterHierarchyButton button)
		{
			buttons.Remove(button);
			(TimelineController.Instance as TimelineEditor).RemoveChapter(button.Chapter);
		}
	}
}
