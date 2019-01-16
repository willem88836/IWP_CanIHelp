using IWPCIH.EventTracking;
using IWPCIH.VRMenu;
using System;
using UnityEngine;

namespace IWPCIH.TimelineEvents
{
	[RequireComponent(typeof(GridLayoutGroup3D))]
	public class MultipleChoiceMenu : BaseEvent
	{
		public class MultipleChoiceData : TimelineEventData
		{
			public string Question = "";
			public string[] Answers = new string[0];
			public string[] Chapters = new string[0];
		}
		public override Type EventType { get { return typeof(MultipleChoiceData); } }


		public MultipleChoiceOption QuestionPrefab;
		public MultipleChoiceOption ChoicePrefab;
		public float CameraDistance;

		private MultipleChoiceData myData;


		public override void Invoke()
		{
			myData = (MultipleChoiceData)Event;

			MultipleChoiceOption question = Instantiate(QuestionPrefab, transform);
			question.SetText(myData.Question);

			for (int i = 0; i < myData.Answers.Length; i++)
			{
				string answer = myData.Answers[i];
				MultipleChoiceOption option = Instantiate(ChoicePrefab, transform);
				option.SetText(answer);
				option.Index = i;
				option.OnClick += OnClick;
			}

			GridLayoutGroup3D layoutGroup3D = GetComponent<GridLayoutGroup3D>();
			transform.position = transform.position + (Vector3.up * layoutGroup3D.Spacing.Y * myData.Answers.Length / 2);
			Camera main = Camera.main;
			transform.position += main.transform.forward * CameraDistance;
			transform.rotation = main.transform.rotation;

			(TimelineController.Instance as TimelineExecuter).TogglePause(true);
		}

		public void OnClick(int index)
		{
			(TimelineController.Instance as TimelineExecuter).TogglePause(false);
			int chapterId = TimelineController.Instance.CurrentTimeline.GetIdOf(myData.Chapters[index]);

			if (chapterId != -1)
			{
				TimelineController.Instance.SwitchChapterTo(chapterId);
			}

			Destroy(gameObject);
		}
	}
}
