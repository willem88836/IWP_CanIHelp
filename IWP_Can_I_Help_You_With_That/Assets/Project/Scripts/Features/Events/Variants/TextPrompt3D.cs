using Framework.Core;
using System;
using UnityEngine;

namespace IWPCIH.TimelineEvents
{
	public class TextPrompt3D : MonoBehaviour, ISelectable
	{
		public TextMesh TextField;
		public Action<TextPrompt3D> OnSelected; 


		public virtual void Select()
		{
			OnSelected.SafeInvoke(this);
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
