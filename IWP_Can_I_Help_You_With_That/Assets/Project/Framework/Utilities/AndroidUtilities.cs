using System;
using UnityEngine;

namespace Framework.Android
{
	public static class AndroidUtilities
	{
		/// <summary>
		///		Returns the path of the external storage.
		///		if there is none it returns an empty string.
		/// </summary>
		public static string GetAndroidExternalStoragePath()
		{
			string path = "";
			try
			{
				AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment");
				path = jc.CallStatic<AndroidJavaObject>("getExternalStorageDirectory").Call<string>("getAbsolutePath");
			}
			catch (Exception e)
			{
				Debug.Log(e.Message);
			}

			Debug.LogFormat("Returned Android Path: ({0})", path);

			return path;
		}
	}
}
