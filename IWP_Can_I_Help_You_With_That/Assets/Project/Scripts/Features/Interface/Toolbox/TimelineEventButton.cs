using IWPCIH.TimelineEvents;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterface
{
	[RequireComponent(typeof(Button))]
	public class TimelineEventButton : MonoBehaviour
	{
		public TimelineEventContainer.EventType Type;

		private Button button;

		private void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(delegate 
			{
				(TimelineController.Instance as TimelineEditor).AddEvent(Type);
			});
		}
	}
}
