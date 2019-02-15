using System.IO;
using Framework.Utils;

namespace Framework.Storage
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

			DirectoryUtilities.ForeachFileAt(path, (FileInfo info) =>
			{
				File.Delete(info.FullName);
			});
		}


		/// <summary>
		///		Saves the provided byte[] to the set path.
		/// </summary>
		public static FileInfo Save(byte[] data, string name)
		{
			if (!Directory.Exists(SavePath))
				Directory.CreateDirectory(SavePath);

			FileEncryption.Encrypt(ref data);
			string path = Path.Combine(SavePath, name);
			path = Path.ChangeExtension(path, Extention);
			File.WriteAllBytes(path, data);

			LoggingUtilities.LogFormat("Saved at: {0}", path);

			return new FileInfo(path);
		}

		/// <summary>
		///		Saves the provided string to the set path.
		/// </summary>
		public static FileInfo Save(string data, string name)
		{
			if (!Directory.Exists(SavePath))
				Directory.CreateDirectory(SavePath);

			FileEncryption.Encrypt(ref data);
			string path = Path.Combine(SavePath, name);
			path = Path.ChangeExtension(path, Extention);
			File.WriteAllText(path, data);

			LoggingUtilities.LogFormat("Saved at: {0}", path);

			return new FileInfo(path);
		}


		/// <summary>
		///		Loads a string from the set path.
		/// </summary>
		public static void Load(string name, out string data)
		{
			string path = Path.ChangeExtension(Path.Combine(SavePath, name), Extention);
			if (File.Exists(path))
			{
				data = File.ReadAllText(path);
				FileEncryption.Decrypt(ref data);
			}
			else
			{
				data = null;
			}
		}

		/// <summary>
		///		Loads a byte[] from the set path.
		/// </summary>
		public static void Load(string name, out byte[] data)
		{
			string path = Path.ChangeExtension(Path.Combine(SavePath, name), Extention);
			if (File.Exists(path))
			{
				data = File.ReadAllBytes(path);
				FileEncryption.Decrypt(ref data);
			}
			else
			{
				data = null;
			}
		}
	}
}
