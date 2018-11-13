using System.IO;

namespace IWPCIH.Storage
{
	public static class SaveLoad
	{
		public static string SavePath;

		/// <summary>
		///		Removes all files at the savepath.
		/// </summary>
		public static void CleanPath()
		{
			Utilities.ForeachFileAt(SavePath, (FileInfo info) =>
			{
				File.Delete(info.FullName);
			});
		}

		/// <summary>
		///		Saves the provided data string to the set path.
		/// </summary>
		public static FileInfo Save(string data, string name)
		{
			string path = SavePath + name;

			if (!Directory.Exists(SavePath))
				Directory.CreateDirectory(SavePath);

			File.WriteAllText(path, data);

			UnityEngine.Debug.Log("Saved at: " + path);

			return new FileInfo(path);
		}

		/// <summary>
		///		Loads a data string from the set path.
		/// </summary>
		public static string Load(string name)
		{
			string path = SavePath + name;
			if (File.Exists(path))
				return File.ReadAllText(path);
			else
				return "";
		}
	}
}
