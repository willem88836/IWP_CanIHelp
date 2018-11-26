using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Framework.Core
{
	public static class Utilities
	{

		#region Shuffle

		/// <summary>
		///		Returns a shuffled version of the provided list.
		/// </summary>
		public static List<T> Shuffle<T>(List<T> list)
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
		public static T[] Shuffle<T>(T[] range)
		{
			// TODO: Make this less tacky.
			List<T> list = new List<T>(range);
			list = Shuffle(list);
			return list.ToArray();
		}
		/// <summary>
		///		Returns a list containing a shuffled version of the range.
		/// </summary>
		public static List<T> ShuffleToList<T>(T[] range)
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
		public static T[] ShuffleToArray<T>(List<T> list)
		{
			// TODO: Make this less tacky.
			List<T> shuffledList = Shuffle(list);
			return shuffledList.ToArray();
		}

		#endregion	


		#region SafeInvokes

		// Ty 8D
		public static void SafeInvoke(this Action a)
		{
			if (a != null)
			{ a.Invoke(); }
		}
		public static void SafeInvoke<T>(this Action<T> a, T t)
		{
			if (a != null)
			{ a.Invoke(t); }
		}
		public static void SafeInvoke<T0, T1>(this Action<T0, T1> a, T0 t0, T1 t1)
		{
			if (a != null)
			{ a.Invoke(t0, t1); }
		}
		public static void SafeInvoke<T0, T1, T2>(this Action<T0, T1, T2> a, T0 t0, T1 t1, T2 t2)
		{
			if (a != null)
			{ a.Invoke(t0, t1, t2); }
		}
		public static void SafeInvoke<T0, T1, T2, T3>(this Action<T0, T1, T2, T3> a, T0 t0, T1 t1, T2 t2, T3 t3)
		{
			if (a != null)
			{ a.Invoke(t0, t1, t2, t3); }
		}

		public static TReturn SafeInvoke<TReturn>(this Func<TReturn> a)
		{
			return a == null ? default(TReturn) : a.Invoke();
		}
		public static TReturn SafeInvoke<T0, TReturn>(this Func<T0, TReturn> a, T0 t0)
		{
			return a == null ? default(TReturn) : a.Invoke(t0);
		}
		public static TReturn SafeInvoke<T0, T1, TReturn>(this Func<T0, T1, TReturn> a, T0 t0, T1 t1)
		{
			return a == null ? default(TReturn) : a.Invoke(t0, t1);
		}
		public static TReturn SafeInvoke<T0, T1, T2, TReturn>(this Func<T0, T1, T2, TReturn> a, T0 t0, T1 t1, T2 t2)
		{
			return a == null ? default(TReturn) : a.Invoke(t0, t1, t2);
		}

		#endregion


		#region TransformForeach

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

		#endregion


		#region DirectoryForeach

		/// <summary>
		///		Executes action at every folder within the provided path
		///		and the folders within those folders.
		/// </summary>
		public static void ForeachFolderIn(string path, Action<string> action, Action<string> onFinish = null)
		{
			action.Invoke(path);

			string[] folders = Directory.GetDirectories(path);

			for (int i = 0; i < folders.Length; i++)
			{
				string current = folders[i];
				ForeachFolderIn(current, action, onFinish);
			}

			onFinish.SafeInvoke(path);
		}
		/// <summary>
		///		Executes action at every folder withinthe provided path in reversed order
		///		and the folders within those folders.
		/// </summary>
		public static void ReversedForeachFolderIn(string path, Action<string> action, Action<string> onStart = null)
		{
			onStart.SafeInvoke(path);

			string[] folders = Directory.GetDirectories(path);

			for (int i = folders.Length - 1; i >= 0; i--)
			{
				string current = folders[i];
				ReversedForeachFolderIn(current, action, onStart);
			}

			action.Invoke(path);
		}
		/// <summary>
		///		Executes action at every folder within the provided path.
		/// </summary>
		public static void ForeachFolderAt(string path, Action<string> action, Action<string> onFinish = null)
		{
			string[] folders = Directory.GetDirectories(path);

			for (int i = 0; i < folders.Length; i++)
			{
				string current = folders[i];
				action.Invoke(current);
			}

			onFinish.SafeInvoke(path);
		}
		/// <summary>
		///		Executes action at every folder within the provided path in reversed order.
		/// </summary>
		public static void ReversedForeachFolderAt(string path, Action<string> action, Action<string> onStart = null)
		{
			onStart.SafeInvoke(path);

			string[] folders = Directory.GetDirectories(path);

			for (int i = folders.Length - 1; i >= 0; i--)
			{
				string current = folders[i];
				action.Invoke(current);
			}
		}

		
		/// <summary>
		///		Executes action for every file within the provided path in alphabetical order.
		/// </summary>
		public static void ForeachFileAt(string path, Action<FileInfo> action)
		{
			string[] fileNames = Directory.GetFiles(path);

			foreach (string n in fileNames)
			{
				FileInfo info = new FileInfo(n);
				action.Invoke(info);
			}
		}
		/// <summary>
		///		Executes action for every file within 
		///		the provided path in reversed alphabetical order.
		/// </summary>
		public static void ReversedForeachFileAt(string path, Action<FileInfo> action)
		{
			Debug.Log(path);

			string[] fileNames = Directory.GetFiles(path);

			for (int i = fileNames.Length - 1; i >= 0; i--)
			{
				string n = fileNames[i];
				FileInfo info = new FileInfo(n);
				action.Invoke(info);
			}
		}

		#endregion
	}
}
