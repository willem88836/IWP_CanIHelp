using UnityEngine;
using IWPCIH.EventTracking;
using System;

namespace IWPCIH.TimelineEvents
{
	public class CropEvent : BaseEvent
	{
		public override Type EventType { get { return typeof(CropEventData); } }


		public class CropEventData : TimelineEventData
		{
			public float Time = 0;
		}

		public override void Invoke()
		{

			Debug.Log("crop event is invoked!");
		}
	}
}
