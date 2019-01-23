using Framework.Core;
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
			public string[] Result = new string[0];
		}
		public override Type EventType { get { return typeof(MultipleChoiceData); } }

		public MultipleChoiceOption MessageObject;
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
			layoutGroup3D.ForceUpdate();

			// TODO: For some reason the object spawns at an offset. Needs fixin!
			//Debug.Log(TransformSingleton.Instance);
			Transform orientation = TransformSingleton.Instance ?? Camera.main.transform;
			transform.rotation = orientation.rotation;
			transform.position += orientation.forward * CameraDistance + (Vector3.up * layoutGroup3D.Spacing.Y * myData.Answers.Length / 2);
			//Debug.DrawRay(orientation.position, orientation.forward, Color.red, CameraDistance);
			//Debug.DrawLine(orientation.position, orientation.forward * CameraDistance + (Vector3.up * layoutGroup3D.Spacing.Y * myData.Answers.Length / 2), Color.red, 10);

			(TimelineController.Instance as TimelineExecuter).TogglePause(true);
		}

		public void OnClick(int index)
		{
			string result = myData.Result[index];

			// HACK: this should not be stored as a string. OR this should not be filled in as a string. Try enum or something.
			if (result.StartsWith("switchto:"))
			{
				result = result.Substring(9);
				int chapterId = TimelineController.Instance.CurrentTimeline.GetIdOf(result);

				if (chapterId != -1)
				{
					TimelineController.Instance.SwitchChapterTo(chapterId);
				}

				StopMultipleChoice();
			}
			else if (result.StartsWith("showmessage:"))
			{
				transform.ReversedForeach((Transform child) => { Destroy(child.gameObject); });

				result = result.Substring(12);
				MultipleChoiceOption o = Instantiate(MessageObject, transform);
				o.SetText(result);
				o.OnClick += (int id) => { StopMultipleChoice(); };
			}
			else
			{
				StopMultipleChoice();
			}
		}

		public void StopMultipleChoice()
		{
			(TimelineController.Instance as TimelineExecuter).TogglePause(false);
			Destroy(gameObject);
		}
	}
}
