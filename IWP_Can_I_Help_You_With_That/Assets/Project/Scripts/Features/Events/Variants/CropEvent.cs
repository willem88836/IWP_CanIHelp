using UnityEngine;
using IWPCIH.EventTracking;

namespace IWPCIH.TimelineEvents
{
	public class CropEvent : BaseEvent
	{
		public class CropEventData : TimelineEventData
		{
			public float Time;
		}

		public override void Invoke()
		{
			Debug.Log("crop event is invoked!");
		}
	}
}
