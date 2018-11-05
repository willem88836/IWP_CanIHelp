using System.IO;
using IWPCIH.EventTracking;

public static class SaveLoad
{
	public static string SavePath;

	public static FileInfo Save(Timeline timeline, string name)
	{
		string s_timeline = TimelineSerializer.Serialize(timeline);

		string path = SavePath + name;
		File.WriteAllText(path, s_timeline);

		return new FileInfo(path);
	}


	public static Timeline Load(string name)
	{
		string path = SavePath + name;
		string s_timeline = File.ReadAllText(path);

		Timeline timeline = TimelineSerializer.Deserialize(s_timeline);

		return timeline;
	}
}
