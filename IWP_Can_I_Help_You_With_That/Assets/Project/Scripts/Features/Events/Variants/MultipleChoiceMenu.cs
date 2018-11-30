using UnityEngine;
using IWPCIH.EventTracking;
using System;

namespace IWPCIH.TimelineEvents
{
	public class MultipleChoiceMenu : BaseEvent
	{
		public override Type EventType { get { return typeof(MultipleChoiceData); } }

		public class MultipleChoiceData : TimelineEventData
		{
			public string Question = "";
			public string Answer1 = "";
			public string Answer2 = "";
			public string Answer3 = "";
			public string Answer4 = "";
		}

		public override void Invoke()
		{

			Debug.Log("multiplechoice event is invoked!");
		}
	}
}
