using IWPCIH.EventTracking;
using System;

namespace IWPCIH.TimelineEvents
{
	public class MultipleChoiceMenu : BaseEvent
	{
		public class MultipleChoiceData : TimelineEventData
		{
			public string Question = "";
			public string[] Answers = new string[0];
		}
		public override Type EventType { get { return typeof(MultipleChoiceData); } }


		public MultipleChoiceOption ChoicePrefab;


		public override void Invoke()
		{
			MultipleChoiceData myData = (MultipleChoiceData)Event;

			Spawn(myData.Question);

			foreach (string answer in myData.Answers)
			{
				Spawn(answer);
			}
		}

		private void Spawn(string text)
		{
			MultipleChoiceOption option = Instantiate(ChoicePrefab, transform);
			option.SetText(text);
		}
	}
}
