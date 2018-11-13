using UnityEngine;
using IWPCIH.EventTracking;
using IWPCIH.TimelineEvents;
using IWPCIH.EditorInterface.Components;
using IWPCIH.EditorInterface;

namespace IWPCIH
{
	public class TimelineController : MonoBehaviour
	{
		private string SAVEPATH { get { return Application.temporaryCachePath + "/TimelineSaveData/"; } }

		public static TimelineController instance;

		public Interface ComponentInterface;
		public ChapterHierarchyController ChapterHierarchy;

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
			for (int i = 0; i < 10; i++)
				AddChapter("chapter " + i);
			AddEvent(EventContainer.EventType.CropStart);
		}


		public void SwitchChapterTo(int i)
		{
			currentChapter = timeline.GetChapter(i);
			ComponentInterface.Load(currentChapter);
			Debug.LogFormat("Switching to Chapter {0}", i);
		}

		#region ChapterIteration

		public void AddChapter(string name)
		{
			int index = timeline.ChapterCount;
			TimelineChapter chapter = new TimelineChapter(index, name, "VideoName"); // TODO: get a reference to the video name.
			timeline.AddChapter(chapter);
			SwitchChapterTo(index);

			ChapterHierarchy.AddChapter(chapter);

			Debug.LogFormat("added chapter: {0}", name);
		}
		public void RemoveChapter(TimelineChapter chapter)
		{
			timeline.RemoveChapter(chapter);

			if (chapter == currentChapter)
				SwitchChapterTo(timeline.GetFirst().Id);
		}

		#endregion	

		#region EventIteration

		public TimelineEventData AddEvent(EventContainer.EventType type)
		{
			TimelineEventData timelineEvent = EventContainer.CreateEventOfType(type);
			timelineEvent.Id = currentChapter.EventCount;
			timelineEvent.Type = type;

			currentChapter.AddEvent(timelineEvent);
			ComponentInterface.Spawn(timelineEvent);

			Debug.LogFormat("Added Event (Id: {0}) of type {1}", timelineEvent.Id.ToString(), timelineEvent.Type.ToString());

			return timelineEvent;
		}
		public void RemoveEvent(TimelineEventData data)
		{
			currentChapter.RemoveEvent(data);

			Debug.LogFormat("Removed Event (Id: {0}) of type {1}", data.Id.ToString(), data.Type.ToString());
		}
		public void UpdateEvent(TimelineEventData updatedEvent)
		{
			currentChapter.UpdateEvent(updatedEvent);

			Debug.LogFormat("Updated Event (Id: {0}) of type {1}", updatedEvent.Id.ToString(), updatedEvent.Type.ToString());
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
