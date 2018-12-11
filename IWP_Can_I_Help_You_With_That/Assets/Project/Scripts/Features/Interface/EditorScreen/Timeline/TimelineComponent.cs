using System;
using IWPCIH.EventTracking;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace IWPCIH.EditorInterface.Components
{
	[RequireComponent(typeof(Button))]
	public class TimelineComponent : MonoBehaviour
	{
		private Interface parent;
		private TimelineEventData data;


		private bool isClicked = false;


		float left;
		float right;


		internal void Initialize(Interface parent, TimelineEventData data)
		{
			this.parent = parent;
			this.data = data;

			GetComponent<Button>().onClick.AddListener(delegate { isClicked = !isClicked; });


			Rect parentRect = GetComponentInParent<RectTransform>().rect;
			left = parentRect.position.x - parentRect.width / 2;
			right = parentRect.position.x + parentRect.width / 2;
		}


		private void Update()
		{
			if (!isClicked)
				return;
		
			float x = Mathf.Clamp(Input.mousePosition.x, left, right);

			transform.position = new Vector3(x, transform.position.y, transform.position.z);

			float t = x / (right - left);
			t *= TimelineController.Instance.CurrentChapter.VideoLength;

			FieldInfo info = data.GetType().GetField("InvokeTime");
			info.SetValue(data, t);
		}
	}
}
