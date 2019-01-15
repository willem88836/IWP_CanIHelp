using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Framework.Utils
{
	/// <summary>
	///		Contains utilties for shuffling lists.
	/// </summary>
	public static class ShuffleUtilities
	{
		/// <summary>
		///		Returns a shuffled version of the provided list.
		/// </summary>
		public static List<T> Shuffle<T>(this List<T> list)
		{
			List<T> shuffledList = new List<T>();
			for (int i = list.Count; i > 0; i--)
			{
				int index = UnityEngine.Random.Range(0, i);
				shuffledList.Add(list[index]);
				list.RemoveAt(index);
			}
			return shuffledList;
		}

		/// <summary>
		///		Returns a shuffled version of the provided range.
		/// </summary>
		public static T[] Shuffle<T>(this T[] range)
		{
			// TODO: Make this less tacky.
			List<T> list = new List<T>(range);
			list = Shuffle(list);
			return list.ToArray();
		}

		/// <summary>
		///		Returns a list containing a shuffled version of the range.
		/// </summary>
		public static List<T> ShuffleToList<T>(this T[] range)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < range.Length; i++)
			{
				T item = range[i];
				int index = Random.Range(0, list.Count);
				list.Insert(index, item);
			}
			return list;
		}

		/// <summary>
		///		Returns an array containing a shuffled version of the list.
		/// </summary>
		public static T[] ShuffleToArray<T>(this List<T> list)
		{
			// TODO: Make this less tacky.
			List<T> shuffledList = Shuffle(list);
			return shuffledList.ToArray();
		}
	}
}
