using System.IO;
using Framework.Core;

namespace IWPCIH.Storage
{
	public static class SaveLoad
	{
		public static string SavePath;
		public static string Extention;

		/// <summary>
		///		Removes all files at the savepath.
		/// </summary>
		internal static void CleanPath(string path = "")
		{
			if (path == "")
				path = SavePath;

			Utilities.ForeachFileAt(path, (FileInfo info) =>
			{
				File.Delete(info.FullName);
			});
		}

		/// <summary>
		///		Saves the provided data string to the set path.
		/// </summary>
		public static FileInfo Save(string data, string name)
		{
			if (!Directory.Exists(SavePath))
				Directory.CreateDirectory(SavePath);

			string path = Path.Combine(SavePath, name);
			path = Path.ChangeExtension(path, Extention);
			File.WriteAllText(path, data);

			UnityEngine.Debug.Log("Saved at: " + path);

			return new FileInfo(path);
		}

		/// <summary>
		///		Loads a data string from the set path.
		/// </summary>
		public static string Load(string name)
		{
			string path = Path.ChangeExtension(Path.Combine(SavePath, name), Extention);
			if (File.Exists(path))
				return File.ReadAllText(path);
			else
				return "";
		}
	}
}
