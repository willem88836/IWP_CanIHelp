using Framework.Zipping;
using IWPCIH.EventTracking;
using System.IO;
using UnityEngine;

namespace IWPCIH.Storage
{
	// TODO: Clean up this monstrosity of a script ;-;
	// TODO: Make loading and saving faster! This is rediculous
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
		///		Saves a compiled project to the provided path.
		///		Should only be used in the editor.
		/// </summary>
		public void HardSave(Timeline timeline, string path)
		{
			Debug.LogFormat("Attempting to load save file to ({0})", path);

			if (Directory.Exists(BuildExtractPath))
				SaveLoad.CleanPath(BuildExtractPath);
			else
				Directory.CreateDirectory(BuildExtractPath);

			// TODO: Make this aSync
			HardSavePath = path;
			SoftSave(timeline);

			// Copies timeline data to buildpath.
			string timelineFileName = Path.ChangeExtension(timeline.Name, TIMELINEEXTENTION);
			File.Copy(Path.Combine(SoftSavePath, timelineFileName), Path.Combine(BuildExtractPath, timelineFileName));

			// Copies all videos to buildpath.
			timeline.ForEach((TimelineChapter chapter) =>
			{
				string videoName = Path.Combine(BuildExtractPath, Path.GetFileName(chapter.VideoName));
				File.Copy(chapter.VideoName, videoName);
			});

			// creates filepath.
			string target = Path.ChangeExtension((Path.Combine(HardSavePath, timeline.Name)), PROJECTEXTENTION);
			if (File.Exists(target))
				File.Delete(target);

			// zips the file.
			SimpleZipper zipper = new SimpleZipper(BuildExtractPath, target);
			zipper.Zip();

			SaveLoad.CleanPath(BuildExtractPath);
		}
		
		/// <summary>
		///		Loads a complete project.
		/// </summary>
		public void HardLoad(ref Timeline timeline, string path)
		{
			Debug.LogFormat("Attempting to load file from: ({0}) to ({1})", path, SoftSavePath);

			if (!Directory.Exists(SoftSavePath))
				Directory.CreateDirectory(SoftSavePath);
			else
				SaveLoad.CleanPath(SoftSavePath);

			SimpleZipper zipper = new SimpleZipper(SoftSavePath, path);
			zipper.Unzip();

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
