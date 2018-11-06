using UnityEngine;
using IWPCIH.EventTracking;
using IWPCIH.TimelineEvents;

namespace IWPCIH
{
	public class TimelineController : MonoBehaviour
	{
		public static TimelineController instance;


		private Timeline timeline;
		private TimelineChapter currentChapter;


		private void Awake()
		{
			if (instance != null)
			{
				Destroy(instance.gameObject);
			}

			instance = this;


			timeline = new Timeline();

			SaveLoad.SavePath = Application.temporaryCachePath  + "/TimelineSaveData/";

			Save();
		}


		public void AddChapter(string videoName)
		{
			TimelineChapter chapter = new TimelineChapter(videoName);
			timeline.AddChapter(chapter);
			SwitchChapterTo(timeline.ChapterCount - 1);

			Debug.LogFormat("added chapter: {0}", videoName);
		}

		public void SwitchChapterTo(int i)
		{
			currentChapter = timeline.ChapterAt(i);
		}


		public TimelineEvent AddEvent(EventContainer.EventType type)
		{
			TimelineEvent timelineEvent = EventContainer.CreateEventOfType(type);
			timelineEvent.Id = currentChapter.EventCount;
			timelineEvent.Type = type;

			currentChapter.AddEvent(timelineEvent);

			Debug.LogFormat("Add Event: {0}", type.ToString());

			return timelineEvent;
		}

		public void UpdateEvent(TimelineEvent updatedEvent)
		{
			currentChapter.UpdateEvent(updatedEvent);
		}



		public void Save()
		{
			SaveLoad.Save(timeline, "Timeline");
		}


		public void Load()
		{
			timeline = SaveLoad.Load("SaveData");
		}
	}
}
