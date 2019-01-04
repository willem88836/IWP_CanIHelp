using IWPCIH.EventTracking;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterfaceObjects.Components
{
	public sealed class TimelineEventButton : MonoBehaviour
	{
		public Button SelectionButton;
		[NonSerialized] public TimelineEventData TimelineEventData;

		private EditorInterface editorInterface;


		public void SetTime(EditorInterface editorInterface, TimelineEventData timelineEventData, int intervalObjectSpawnInterval, float intervalObjectWidth)
		{
			this.editorInterface = editorInterface;
			this.TimelineEventData = timelineEventData;

			transform.localPosition = Vector3.right * (timelineEventData.InvokeTime / intervalObjectSpawnInterval) * intervalObjectWidth;


			SelectionButton.onClick.RemoveListener(OnSelect);
			SelectionButton.onClick.AddListener(OnSelect);
		}

		private void OnSelect()
		{
			editorInterface.OnEventSelected(TimelineEventData);
		}
	}
}
