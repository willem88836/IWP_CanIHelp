using UnityEngine;

namespace IWPCIH.TimelineEvents
{
	public class CropEvent : BaseEvent
	{
		public class CropEventData : EventTracking.TimelineEvent
		{
			public float Time;
		}


		public override void Invoke()
		{
			Debug.Log("crop event is invoked!");
		}
	}
}
