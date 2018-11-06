using UnityEngine;
using IWPCIH.EventTracking;

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
			currentChapter = chapter;
		}

		public void SwitchChapterTo(int i)
		{
			currentChapter = timeline.ChapterAt(i);
		}


		public TimelineEvent AddEvent(TimelineEvent.EventType type)
		{
			TimelineEvent timelineEvent = new TimelineEvent()
			{
				InvokeTime = 0,
				Type = type,
				Id = currentChapter.EventCount - 1
			};

			currentChapter.AddEvent(timelineEvent);
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
