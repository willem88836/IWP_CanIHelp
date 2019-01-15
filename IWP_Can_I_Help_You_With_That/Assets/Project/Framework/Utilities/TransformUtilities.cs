using System;
using UnityEngine;

namespace Framework.Utils
{ 
	/// <summary>
	///		Contains utility methods for Transform.
	/// </summary>
	public static class TransformUtilities
	{
		/// <summary>
		///		Returns last child of this object. 
		///		If none exist, returns transform.
		/// </summary>
		public static Transform GetLastChild(this Transform transform)
		{
			int i = transform.childCount - 1;
			if (i < 0)
				return transform;
			else
				return transform.GetChild(i);
		}

		/// <summary>
		///		Iterates through all children.
		/// </summary>
		public static void Foreach(this Transform transform, Action<Transform> action)
		{
			var count = transform.childCount;
			for (var i = 0; i < count; i++)
			{
				var child = transform.GetChild(i);

				action.Invoke(child);
				Foreach(child, action);
			}
		}

		/// <summary>
		///		Iterates through all children in reversed order.
		/// </summary>
		public static void ReversedForeach(this Transform transform, Action<Transform> action)
		{
			var count = transform.childCount;
			for (var i = count - 1; i >= 0; i--)
			{
				var child = transform.GetChild(i);

				ReversedForeach(child, action);
				action.Invoke(child);
			}
		}
	}
}
