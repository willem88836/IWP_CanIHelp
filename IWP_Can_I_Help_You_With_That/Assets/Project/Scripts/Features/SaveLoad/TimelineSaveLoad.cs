using Framework.Core;
using IWPCIH.EventTracking;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Ionic.Zip;

namespace IWPCIH.Storage
{
	/// <summary>
	///		Saves and loads a timelineproject.
	/// </summary>
	public class TimelineSaveLoad
	{
		public const string TIMELINEEXTENTION = ".tl";
		public const string PROJECTEXTENTION = ".tld";

		// DON'T YOU DARE CHANGE THIS PATHS WHEN YOU DON'T KNOW WHAT YOU'RE DOING. IT CAN KILL YOU PC.
		public static string BuildExtractPath { get { return Path.Combine(Application.temporaryCachePath, "Build/"); } }
		public static string SoftSavePath { get { return Path.Combine(Application.temporaryCachePath, "TimelineSaveData/"); } }
		public static string HardSavePath { get; private set; } 


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
			File.Copy(Path.Combine(SoftSavePath, name), Path.Combine(BuildExtractPath, name), true);

			// all interface files. 
			Utilities.ForeachFileAt(SoftSavePath, (FileInfo info) =>
			{
				if (!chapterNames.Contains(info.Name))
					return;

				File.Copy(info.FullName, Path.Combine(BuildExtractPath, info.Name), true);
				chapterNames.Remove(chapterNames.Find((string s) => s == info.Name));
			});

			// All video files. 
			timeline.ForEach((TimelineChapter c) => 
			{
				return; // TODO: remove this once the video filenames are properly set. 
				string target = Path.Combine(BuildExtractPath, Path.GetFileName(c.VideoName));
				File.Copy(c.VideoName, target);
			});

			// TODO: set proper save path.
			string zipName = Path.ChangeExtension((Path.Combine(HardSavePath, timeline.Name)), PROJECTEXTENTION);
			if (File.Exists(zipName))
				File.Delete(zipName);
			
			ZipFile zip = new ZipFile(zipName);
			zip.AddDirectory(BuildExtractPath);

			zip.Save();
			zip.Dispose();

			SaveLoad.CleanPath(BuildExtractPath);
		}

		// TODO: add error reference when no file exists. 
		/// <summary>
		///		Loads a compiled project.
		/// </summary>
		public void HardLoad(ref Timeline timeline, string path)
		{
			if (!Directory.Exists(SoftSavePath))
				Directory.CreateDirectory(SoftSavePath);
			else
				SaveLoad.CleanPath(SoftSavePath);

			ZipFile zip = ZipFile.Read(path);
			zip.ExtractAll(SoftSavePath, ExtractExistingFileAction.OverwriteSilently);
			zip.Dispose();

			SoftLoad(ref timeline, Path.GetFileNameWithoutExtension(path));

			timeline.ForEach((TimelineChapter tlc) =>
			{
				tlc.VideoName = Path.Combine(SoftSavePath, Path.GetFileName(tlc.VideoName));
			});
		}
	}
}
