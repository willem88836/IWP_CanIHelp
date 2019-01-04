using IWPCIH.TimelineEvents;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterfaceObjects.Menu
{
	[RequireComponent(typeof(Button))]
	public class NewTimelineEventButton : MonoBehaviour
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
