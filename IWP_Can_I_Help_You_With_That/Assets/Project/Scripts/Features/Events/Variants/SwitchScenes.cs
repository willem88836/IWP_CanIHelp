using IWPCIH.EventTracking;
using System;

namespace IWPCIH.TimelineEvents
{
	public sealed class SwitchScenes : BaseEvent
	{
		public class SwitchScenesData : TimelineEventData
		{
			public string ChapterName;
		}

		public override Type EventType { get { return typeof(SwitchScenesData); } }

		public override void Invoke()
		{
			SwitchScenesData data = Event as SwitchScenesData;
			TimelineExecuter executer = TimelineController.Instance as TimelineExecuter;
			int sceneId = executer.CurrentTimeline.GetIdOf(data.ChapterName);
			executer.SwitchChapterTo(sceneId);
		}
	}
}
