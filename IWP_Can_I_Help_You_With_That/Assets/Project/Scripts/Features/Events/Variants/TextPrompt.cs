using IWPCIH.EventTracking;
using System;
using UnityEngine;

namespace IWPCIH.TimelineEvents
{
	public sealed class TextPrompt : BaseEvent
	{
		public class TextPromptData : TimelineEventData
		{
			public string Message = "";
			public string Result = "";
		}

		public override Type EventType { get { return typeof(TextPromptData); } }

		public TextPrompt3D PromptObject;
		public float CameraDistance;

		private TextPromptData myData;


		public override void Invoke()
		{
			(TimelineController.Instance as TimelineExecuter).TogglePause(true);

			myData = Event as TextPromptData;

			TextPrompt3D prompt = Instantiate(PromptObject, transform);
			prompt.SetText(myData.Message);
			prompt.OnSelected += OnPromptClicked;

			Camera main = Camera.main;
			transform.position = transform.position + main.transform.forward * CameraDistance;
			transform.rotation = main.transform.rotation;
		}

		private void OnPromptClicked(TextPrompt3D text)
		{
			// TODO: THis is copied from multiplechoice menu. Add this into some generically accessible script.
			string result = myData.Result;

			// HACK: this should not be stored as a string. OR this should not be filled in as a string. Try enum or something.
			if (result.StartsWith("switchto:"))
			{
				result = result.Substring(9);
				int chapterId = TimelineController.Instance.CurrentTimeline.GetIdOf(result);

				if (chapterId != -1)
				{
					TimelineController.Instance.SwitchChapterTo(chapterId);
				}

				StopPrompt();
			}
			else
			{
				StopPrompt();
			}
		}

		public void StopPrompt()
		{
			(TimelineController.Instance as TimelineExecuter).TogglePause(false);
			Destroy(gameObject);
		}
	}
}
