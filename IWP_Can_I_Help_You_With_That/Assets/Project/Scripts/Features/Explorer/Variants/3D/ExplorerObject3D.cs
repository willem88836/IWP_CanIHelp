using UnityEngine;

namespace IWPCIH.Explorer
{
	public abstract class ExplorerObject3D : ExplorerObject, ISelectable
	{
		public int TextWidth = 8;
		public int MaxRows = 3;

		public abstract void Select();

		protected string WrapText(string text)
		{
			// TODO: Improve this stuff. this cuts words in half.
			int loops = Mathf.FloorToInt(text.Length / TextWidth);
			loops = Mathf.Min(loops, MaxRows);

			for (int i = TextWidth; i < text.Length; i += TextWidth)
			{
				if (i % TextWidth >= loops)
					break;

				int index = i + i / TextWidth;
				if (index < text.Length)
					text = text.Insert(index, "\n");
			}

			return text;
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
							Debug.Log("Selected object with mouse!");
							Select();
						}
					}
				}
			}
		#endif
	}
}
