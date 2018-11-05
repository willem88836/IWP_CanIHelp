using IWPCIH.EventTracking;
using UnityEngine;

namespace Assets.Project.Scripts.Timeline_Implementation
{
	public class TimelineTest : MonoBehaviour
	{
		public Timeline timeline;

		private void Start()
		{
			timeline = new Timeline();

			for (int i = 0; i < 10; i++)
			{
				TimelineChapter chapter = new TimelineChapter("videoname:_" + i);
				timeline.AddChapter(chapter);

				for (int j = 0; j < 10; j++)
				{
					TimelineEvent newEvent = new TimelineEvent(j * i, TimelineEvent.EventType.CropStart);
					chapter.AddEvent(newEvent);
				}
			}
		}
	}
}
