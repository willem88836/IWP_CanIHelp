using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterfaceObjects.Components
{
	public sealed class TimelineIntervalObject : MonoBehaviour
	{
		private const string FORMAT = "{0}:{1}";
		public Text TimeTextField;

		public void SetTime(int time)
		{
			TimeTextField.text = string.Format(FORMAT, time / 60, time % 60);
		}
	}
}
