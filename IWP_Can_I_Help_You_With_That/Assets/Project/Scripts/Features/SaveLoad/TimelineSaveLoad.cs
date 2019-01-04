using Framework.Core;
using IWPCIH.EventTracking;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace IWPCIH.Storage
{
	// TODO: Clean up this monstrosity of a script ;-;
	/// <summary>
	///		Saves and loads a timelineproject.
	/// </summary>
	public class TimelineSaveLoad
	{
		public const string TIMELINEEXTENTION = ".tl";
		public const string PROJECTEXTENTION = ".tld";

		// DON'T YOU DARE CHANGE THIS PATHS WHEN YOU DON'T KNOW WHAT YOU'RE DOING. IT CAN KILL YOU PC.
		// TODO: Choose your own cache path? 
		public static string BuildExtractPath { get { return Path.Combine(Application.temporaryCachePath, "Build/"); } }
		public static string SoftSavePath { get { return Path.Combine(Application.temporaryCachePath, "TimelineSaveData/"); } }
		public static string HardSavePath { get; private set; }

		// TODO: custom password protection? 
		private const string DEFAULTPASSWORD = "*uO5LtEe!n6CwGfuEVa5YukaFRs9OlgRGVByjUCuC@gkZG0MM$cJhk!cPSHR&mdxa";


		/// <summary>
		///		Saves merely the timeline data from appdata. 
		/// </summary>
		public void SoftSave(Timeline timeline)
		{
			SaveLoad.SavePath = SoftSavePath;
			SaveLoad.Extention = TIMELINEEXTENTION;
			string s_timeline = TimelineSerializer.Serialize(timeline);
			SaveLoad.Save(s_timeline, timeline.Name);
		}

		/// <summary>
		///		Loads merely the timeline data from appdata.
		/// </summary>
		public void SoftLoad(ref Timeline timeline, string saveName)
		{
			SaveLoad.SavePath = SoftSavePath;
			SaveLoad.Extention = TIMELINEEXTENTION;
			string s_timeline = SaveLoad.Load(saveName);
			Timeline tl = TimelineSerializer.Deserialize(s_timeline);
			timeline.Chapters = tl.Chapters;
		}

		
		/// <summary>
		///		Saves a compiled project.
		///		Can only be used inside the editor.
		/// </summary>
		public void HardSave(Timeline timeline, string path)
		{
			if (Directory.Exists(BuildExtractPath))
				SaveLoad.CleanPath(BuildExtractPath);

			// TODO: Make this aSync
			HardSavePath = path;
			SoftSave(timeline);

			// Copies all relevant data to the build folder. 
			List<string> chapterNames = new List<string>();
			timeline.ForEach((TimelineChapter c) => { chapterNames.Add(c.Name); });
			chapterNames.Sort(delegate (string x, string y) { return x.CompareTo(y); });

			if (!Directory.Exists(BuildExtractPath))
				Directory.CreateDirectory(BuildExtractPath);

			// The timeline file.
			string name = Path.ChangeExtension(timeline.Name, TIMELINEEXTENTION);
			// HACK: for some reason the first letter of each file is removed. This kinda solves it, but is ugly AF.
			File.Copy(Path.Combine(SoftSavePath, name), Path.Combine(BuildExtractPath, '_' + name), true);

			// all interface files. 
			Utilities.ForeachFileAt(SoftSavePath, (FileInfo info) =>
			{
				if (!chapterNames.Contains(info.Name))
					return;

				// HACK: for some reason the first letter of each file is removed. This kinda solves it, but is ugly AF.
				string fileName = '_' + info.Name;
				File.Copy(info.FullName, Path.Combine(BuildExtractPath, fileName), true);
				chapterNames.Remove(chapterNames.Find((string s) => s == info.Name));
			});

			// All video files. 
			timeline.ForEach((TimelineChapter c) =>
			{
				// HACK: for some reason the first letter of each file is removed. This kinda solves it, but is ugly AF.
				string videoName = Path.GetFileName(c.VideoName);
				videoName = '_' + videoName;
				string target = Path.Combine(BuildExtractPath, videoName);
				File.Copy(c.VideoName, target);
			});


			string zipName = Path.ChangeExtension((Path.Combine(HardSavePath, timeline.Name)), PROJECTEXTENTION);
			if (File.Exists(zipName))
				File.Delete(zipName);


			string outPathname = Path.ChangeExtension((Path.Combine(HardSavePath, timeline.Name)), PROJECTEXTENTION);
			string password = DEFAULTPASSWORD;
			string folderName = BuildExtractPath;

			FileStream fsOut = File.Create(outPathname);
			ZipOutputStream zipStream = new ZipOutputStream(fsOut);

			zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

			zipStream.Password = password;  // optional. Null is the same as not setting. Required if using AES.

			// This setting will strip the leading part of the folder path in the entries, to
			// make the entries relative to the starting folder.
			// To include the full path for each entry up to the drive root, assign folderOffset = 0.
			int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);

			CompressFolder(folderName, zipStream, folderOffset);

			zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
			zipStream.Close();

			SaveLoad.CleanPath(BuildExtractPath);
		}
		
		// Recurses down the folder structure
		 //
		private void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
		{

			string[] files = Directory.GetFiles(path);

			foreach (string filename in files)
			{

				FileInfo fi = new FileInfo(filename);

				string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
				entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
				ZipEntry newEntry = new ZipEntry(entryName);
				newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

				// Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
				// A password on the ZipOutputStream is required if using AES.
				//   newEntry.AESKeySize = 256;

				// To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
				// you need to do one of the following: Specify UseZip64.Off, or set the Size.
				// If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
				// but the zip will be in Zip64 format which not all utilities can understand.
				//   zipStream.UseZip64 = UseZip64.Off;
				newEntry.Size = fi.Length;

				zipStream.PutNextEntry(newEntry);

				// Zip the file in buffered chunks
				// the "using" will close the stream even if an exception occurs
				byte[] buffer = new byte[4096];
				using (FileStream streamReader = File.OpenRead(filename))
				{
					StreamUtils.Copy(streamReader, zipStream, buffer);
				}
				zipStream.CloseEntry();
			}
			string[] folders = Directory.GetDirectories(path);
			foreach (string folder in folders)
			{
				CompressFolder(folder, zipStream, folderOffset);
			}
		}


		public void HardLoad(ref Timeline timeline, string path)
		{
			Debug.LogFormat("Attempting to load file from: ({0}) to ({1})", path, SoftSavePath);

			if (!Directory.Exists(SoftSavePath))
				Directory.CreateDirectory(SoftSavePath);
			else
				SaveLoad.CleanPath(SoftSavePath);


			string archiveFilenameIn = path;
			string password = DEFAULTPASSWORD;
			string outFolder = SoftSavePath;

			ZipFile zf = null;

			try
			{
				FileStream fs = File.OpenRead(archiveFilenameIn);
				zf = new ZipFile(fs);
				if (!string.IsNullOrEmpty(password))
				{
					zf.Password = password;     // AES encrypted entries are handled automatically
				}
				foreach (ZipEntry zipEntry in zf)
				{
					if (!zipEntry.IsFile)
					{
						continue;           // Ignore directories
					}
					string entryFileName = zipEntry.Name;
					// to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
					// Optionally match entrynames against a selection list here to skip as desired.
					// The unpacked length is available in the zipEntry.Size property.

					byte[] buffer = new byte[4096];     // 4K is optimum
					Stream zipStream = zf.GetInputStream(zipEntry);

					// Manipulate the output filename here as desired.
					string fullZipToPath = Path.Combine(outFolder, entryFileName);
					string directoryName = Path.GetDirectoryName(fullZipToPath);
					if (directoryName.Length > 0)
						Directory.CreateDirectory(directoryName);

					// Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
					// of the file, but does not waste memory.
					// The "using" will close the stream even if an exception occurs.
					using (FileStream streamWriter = File.Create(fullZipToPath))
					{
						StreamUtils.Copy(zipStream, streamWriter, buffer);
					}
				}
			}
			finally
			{
				if (zf != null)
				{
					zf.IsStreamOwner = true; // Makes close also shut the underlying stream
					zf.Close(); // Ensure we release resources
				}
			}

			SoftLoad(ref timeline, Path.GetFileNameWithoutExtension(path));

			timeline.ForEach((TimelineChapter tlc) =>
			{
				if (Application.platform == RuntimePlatform.WindowsPlayer 
					|| Application.platform == RuntimePlatform.WindowsEditor)
				{
					tlc.VideoName = Path.Combine(SoftSavePath, Path.GetFileName(tlc.VideoName));
				}
				else if (Application.platform == RuntimePlatform.Android)
				{
					// HACK: for some reason 'Path.GetFileName("")' does not work on Android.
					string[] splittedPath = tlc.VideoName.Split('/', '\\');
					tlc.VideoName = Path.Combine(SoftSavePath, splittedPath[splittedPath.Length - 1]);
				}
				else
				{
					throw new System.Exception("Unpacking at unsupported platform");
				}
			});
		}
	}
}
