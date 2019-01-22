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
			int min = time / 60;
			int sec = time % 60;
			string secStr = sec.ToString().Length == 1 ? "0" + sec : sec.ToString();
			TimeTextField.text = string.Format(FORMAT, min, secStr);
		}
	}
}
