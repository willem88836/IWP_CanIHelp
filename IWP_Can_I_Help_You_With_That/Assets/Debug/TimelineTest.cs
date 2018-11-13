using IWPCIH.EventTracking;
using UnityEngine;

namespace Assets.Project.Scripts.Timeline_Implementation
{
	public class TimelineTest : MonoBehaviour
	{
		public static TimelineTest instance; 

		public Timeline timeline;

		private void Awake()
		{
			instance = this;

			CreateCropTimeline();
		}

		public Timeline CreateCropTimeline()
		{
			timeline = new Timeline();

			for (int i = 0; i < 10; i++)
			{
				TimelineChapter chapter = new TimelineChapter(i, "Chapter 1", "videoname:_" + i);
				timeline.AddChapter(chapter);

				for (int j = 0; j < 10; j++)
				{
					TimelineEventData newEvent = new IWPCIH.TimelineEvents.CropEvent.CropEventData()
					{
						Id = j,
						Type = IWPCIH.TimelineEvents.TimelineEventContainer.EventType.CropStart,
						InvokeTime = i * j
					};

					chapter.AddEvent(newEvent);
				}
			}

			return timeline;
		}
	}
}
