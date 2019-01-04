using UnityEngine;
using Framework.Core;

namespace IWPCIH.VRMenu
{
	public class GridLayoutGroup3D : MonoBehaviour
	{
		public int ColumnCount = 0;
		public Int2 Spacing;




		private int childCount;


		private void OnValidate()
		{
			UpdateLayout();
		}


		private void Update()
		{
			int currentChildCount = transform.childCount;
			if (childCount != currentChildCount)
			{
				childCount = currentChildCount;

				UpdateLayout();
			}
		}

		private void UpdateLayout()
		{
			int y = 0;
			for (int i = 0; i < transform.childCount; i++)
			{
				if (i % ColumnCount == 0 && i != 0)
					y--;

				int x = i % ColumnCount;

				Vector3 position = new Vector3(x * Spacing.X, y * Spacing.Y, 0);
				Transform t = transform.GetChild(i);
				t.position = transform.position + position;

			}
		}
	}
}
