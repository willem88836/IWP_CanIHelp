using Framework.ScriptableObjects.Variables;
using IWPCIH.Explorer;
using IWPCIH.Storage;
using System.IO;
using UnityEngine;

public class SaveLoadPathManager : MonoBehaviour
{
	public StringReference SavePath;
	public StringReference LoadPath;

	[Space]
	public Explorer Explorer;
	public TimelineSaveLoadWrapper timelineSaveLoad;


	public void InitiateSave()
	{
		Explorer.gameObject.SetActive(true);
		Explorer.OnPathSelected -= StartSave;
		Explorer.OnPathSelected += StartSave;
	}

	public void StartSave(string path)
	{
		if (!Directory.Exists(path))
			return;

		Explorer.gameObject.SetActive(false);
		Explorer.OnPathSelected -= StartSave;
		SavePath.Value = path;
		timelineSaveLoad.HardSave();
	}


	public void InitiateLoad()
	{
		Explorer.gameObject.SetActive(true);
		Explorer.OnPathSelected -= StartLoad;
		Explorer.OnPathSelected += StartLoad;
	}

	public void StartLoad(string path)
	{
		if (!File.Exists(path))
			return;
		
		Explorer.gameObject.SetActive(false);
		Explorer.OnPathSelected -= StartLoad;
		LoadPath.Value = path;
		timelineSaveLoad.HardLoad();
	}
}
