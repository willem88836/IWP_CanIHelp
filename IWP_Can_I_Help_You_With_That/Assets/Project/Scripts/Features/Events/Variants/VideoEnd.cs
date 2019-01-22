using IWPCIH.EventTracking;
using System;

namespace IWPCIH.TimelineEvents
{
	public sealed class VideoEnd : BaseEvent
	{

		public override Type EventType { get { return typeof(TimelineEventData); } }

		public override void Invoke()
		{
			(TimelineController.Instance as TimelineExecuter).Stop();
		}
	}
}
