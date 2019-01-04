using Framework.Core;
using IWPCIH.EventTracking;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IWPCIH.EditorInterfaceObjects.Components
{
	public class EditorInterface : MonoBehaviour
	{
		public InterfaceComponent PreviewComponent;
		[Space]
		public Transform TimelineIntervalObjectContainer;
		public TimelineIntervalObject BaseIntervalObject;
		public int IntervalObjectSpawnInterval;
		[Space]
		public Transform TimelineEventButtonContainer;
		public TimelineEventButton baseEventButton;

		private List<TimelineEventButton> eventButtons = new List<TimelineEventButton>();

		public void Initialize(TimelineChapter chapter)
		{
			Clear();

			SpawnIntervalObjects(chapter.VideoLength);
			SpawnEventObjects(chapter);

			PreviewComponent.Clear();
			eventButtons.Clear();
		}

		private void Clear()
		{
			TimelineIntervalObjectContainer.ReversedForeach((Transform child) => Destroy(child));
			TimelineEventButtonContainer.ReversedForeach((Transform child) => Destroy(child));
		}


		private void SpawnIntervalObjects(int videoLength)
		{
			for (int time = 0; time < videoLength; time+= IntervalObjectSpawnInterval)
			{
				Instantiate(BaseIntervalObject, TimelineIntervalObjectContainer).SetTime(time);
			}
		}

		private void SpawnEventObjects(TimelineChapter chapter)
		{
			chapter.Foreach((TimelineEventData data) => Spawn(data));
		}

		public void Spawn(TimelineEventData data)
		{
			TimelineEventButton newButton = Instantiate(baseEventButton, TimelineEventButtonContainer);
			float intervalObjectWidth = BaseIntervalObject.GetComponent<Rect>().width;
			newButton.SetTime(this, data, IntervalObjectSpawnInterval, intervalObjectWidth);
			eventButtons.Add(newButton);
		}


		public void OnEventSelected(TimelineEventData selected)
		{
			PreviewComponent.Initialize(this, selected);
		}

		public void OnTimeChanged(TimelineEventData timelineEventData)
		{
			TimelineEventButton button = eventButtons.Find((TimelineEventButton b) => b.TimelineEventData == timelineEventData);
			if (button != null)
			{
				float intervalObjectWidth = BaseIntervalObject.GetComponent<Rect>().width;
				button.SetTime(this, timelineEventData, IntervalObjectSpawnInterval, intervalObjectWidth);
			}
		}
	}
}
