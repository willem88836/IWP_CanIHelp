using IWPCIH.EditorInterfaceObjects.Components;
using IWPCIH.EditorInterfaceObjects.Menu;
using IWPCIH.EventTracking;
using IWPCIH.TimelineEvents;
using IWPCIH.Storage;
using UnityEngine;

namespace IWPCIH
{
	public sealed class TimelineEditor : TimelineController
	{
		public EditorInterface ComponentInterface;
		public ChapterHierarchyController ChapterHierarchy;


		public override void SwitchChapterTo(int i)
		{
			if (CurrentChapter != null)
				TimelineSaveLoadWrapper.Instance.SoftSave();

			base.SwitchChapterTo(i);

			ComponentInterface.Initialize(CurrentChapter);
		}

		#region ChapterIteration

		public void AddChapter(string name, string videoName)
		{
			int index = CurrentTimeline.ChapterCount;

			// TODO: HIGH PRIO!!! Calculate actual video length.
			int videoLength = 0;

			TimelineChapter chapter = new TimelineChapter(index, name, videoName, videoLength);
			CurrentTimeline.AddChapter(chapter);
			SwitchChapterTo(index);

			ChapterHierarchy.AddChapter(chapter);

			TimelineSaveLoadWrapper.Instance.SoftSave();

			Debug.LogFormat("Added chapter: (name: {0}) using (video: {1})", name, videoName);
		}
		public void RemoveChapter(TimelineChapter chapter)
		{
			CurrentTimeline.RemoveChapter(chapter);

			if (chapter == CurrentChapter)
				SwitchChapterTo(CurrentTimeline.GetFirst().Id);
		}

		#endregion

		#region EventIteration

		public TimelineEventData AddEvent(TimelineEventContainer.EventType type)
		{
			if (CurrentChapter == null)
				return null;

			TimelineEventData timelineEvent = TimelineEventContainer.CreateEventDataOfType(type);
			timelineEvent.Id = CurrentChapter.NextId();
			timelineEvent.Type = type;
			CurrentChapter.AddEvent(timelineEvent);
			ComponentInterface.Spawn(timelineEvent);

			Debug.LogFormat("Added Event (Id: {0}) of type {1}", timelineEvent.Id.ToString(), timelineEvent.Type.ToString());
			return timelineEvent;
		}
		public void RemoveEvent(TimelineEventData data)
		{
			CurrentChapter.RemoveEvent(data);
			Debug.LogFormat("Removed Event (Id: {0}) of type {1}", data.Id.ToString(), data.Type.ToString());
		}
		public void UpdateEvent(TimelineEventData updatedEvent)
		{
			CurrentChapter.UpdateEvent(updatedEvent);
			Debug.LogFormat("Updated Event (Id: {0}) of type {1}", updatedEvent.Id.ToString(), updatedEvent.Type.ToString());
		}

		#endregion

		#region SaveLoadChapterHierarchy

		public override void OnSave()
		{
			// do stuff here if anything specific needs to be saved. 
		}
		public override void OnLoad()
		{
			ChapterHierarchy.Clear();
			CurrentTimeline.ForEach((TimelineChapter chapter) =>
			{
				ChapterHierarchy.AddChapter(chapter);
			});

			SwitchChapterTo(CurrentTimeline.GetFirst().Id);
		}

		#endregion	
	}
}
