using UnityEngine;
using IWPCIH.EventTracking;
using IWPCIH.TimelineEvents;
using IWPCIH.EditorInterface.Components;

namespace IWPCIH
{
	public class TimelineController : MonoBehaviour
	{
		private string SAVEPATH { get { return Application.temporaryCachePath + "/TimelineSaveData/"; } }

		public static TimelineController instance;

		public Interface componentInterface;

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

			SaveLoad.SavePath = SAVEPATH;
			Foo();
		}

		private void Foo()
		{
			AddChapter("chapter 1");
			AddEvent(EventContainer.EventType.CropStart);
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


		#region ChapterIteration

		public TimelineEventData AddEvent(EventContainer.EventType type)
		{
			TimelineEventData timelineEvent = EventContainer.CreateEventOfType(type);
			timelineEvent.Id = currentChapter.EventCount;
			timelineEvent.Type = type;

			currentChapter.AddEvent(timelineEvent);
			componentInterface.Spawn(timelineEvent);

			Debug.LogFormat("Added Event {0} of type {1}", timelineEvent.Id.ToString(), timelineEvent.Type.ToString());

			return timelineEvent;
		}
		public void RemoveEvent(TimelineEventData data)
		{
			currentChapter.RemoveEvent(data);

			Debug.LogFormat("Removed Event {0} of type {1}", data.Id.ToString(), data.Type.ToString());
		}
		public void UpdateEvent(TimelineEventData updatedEvent)
		{
			currentChapter.UpdateEvent(updatedEvent);

			Debug.LogFormat("Updated Event {0} of type {1}", updatedEvent.Id.ToString(), updatedEvent.Type.ToString());
		}

		#endregion

		#region SaveLoad

		public void Save()
		{
			SaveLoad.Save(timeline, "Timeline");
			// TODO: Save interface data.
		}
		public void Load()
		{
			timeline = SaveLoad.Load("SaveData");
		}

		#endregion

	}
}
