using UnityEngine;
using IWPCIH.EventTracking;
using System;
using System.Reflection;

namespace IWPCIH.TimelineEvents
{
	public class MultipleChoiceMenu : BaseEvent
	{
		public class MultipleChoiceData : TimelineEventData
		{
			public string Question = "";
			public string Answer1 = "";
			public string Answer2 = "";
			public string Answer3 = "";
			public string Answer4 = "";
		}
		public override Type EventType { get { return typeof(MultipleChoiceData); } }


		public MultipleChoiceOption ChoicePrefab;


		public override void Invoke()
		{
			MultipleChoiceData myData = (MultipleChoiceData)Event;

			/// TODO: Please implent arrays. I hate myself for having to type this.
			MultipleChoiceOption option = Instantiate(ChoicePrefab, transform);
			option.SetText(myData.Question);
			option = Instantiate(ChoicePrefab, transform);
			option.SetText(myData.Answer1);
			option = Instantiate(ChoicePrefab, transform);
			option.SetText(myData.Answer2);
			option = Instantiate(ChoicePrefab, transform);
			option.SetText(myData.Answer3);
			option = Instantiate(ChoicePrefab, transform);
			option.SetText(myData.Answer4);
		}
	}
}
