using IWPCIH.EventTracking;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.InterfaceFeatures
{
	[RequireComponent(typeof(Button))]
	public class TimelineEventButton : MonoBehaviour
	{
		public TimelineEvent.EventType Type;

		private Button button;

		private void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(delegate 
			{
				TimelineController.instance.AddEvent(Type);
			});
		}
	}
}
