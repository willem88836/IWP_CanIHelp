using Framework.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

//TODO: Derive this from TextPrompt3D
namespace IWPCIH.TimelineEvents
{
	public class MultipleChoiceOption : MonoBehaviour, ISelectable
	{
		public Text TextField;
		[NonSerialized] public int Index = 0;

		public Action<int> OnClick = null;

		public void Select()
		{
			OnClick.SafeInvoke(Index);
		}

		public void SetText(string text)
		{
			TextField.text = text;
		}


#if UNITY_EDITOR
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit info;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out info, float.PositiveInfinity))
				{
					Debug.DrawLine(ray.origin, info.point, Color.red, 1);
					if (info.transform == transform)
					{
						Select();
					}
				}
			}
		}
#endif
	}
}