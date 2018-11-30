using UnityEngine;

namespace IWPCIH.TimelineEvents
{
	public class MultipleChoiceOption : MonoBehaviour
	{
		public TextMesh TextField;

		public void SetText(string text)
		{
			TextField.text = text;
		}
	}
}