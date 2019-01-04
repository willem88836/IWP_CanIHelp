using IWPCIH.EventTracking;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterfaceObjects.Menu
{
	public class ChapterHierarchyButton : MonoBehaviour
	{
		private const string NAMEFORMAT = "{0}_Button_ChapterHierarchy_{1}";

		public Text ButtonLabel;

		public TimelineChapter Chapter { get; private set; }
		private ChapterHierarchyController hierarchyController;


		public void Initialize(ChapterHierarchyController hierarchyController, TimelineChapter chapter)
		{
			this.hierarchyController = hierarchyController;
			this.Chapter = chapter;

			name = string.Format(NAMEFORMAT, chapter.Id, chapter.Name);
			ButtonLabel.text = chapter.Name;
		}

		public void OnClick()
		{
			TimelineController.Instance.SwitchChapterTo(Chapter.Id);
		}

		public void Destroy()
		{
			hierarchyController.RemoveChapter(this);
			Destroy(gameObject);
		}
	}
}
