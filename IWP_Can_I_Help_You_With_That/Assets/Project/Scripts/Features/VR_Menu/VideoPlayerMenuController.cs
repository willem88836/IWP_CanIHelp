using Framework.ScriptableObjects.Variables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IWPCIH.VRMenu
{
	public sealed class VideoPlayerMenuController : MonoBehaviour
	{
		public Explorer.Explorer explorer;
		public StringReference LoadPath;
		public string VideoPlayerSceneName = "VR_VideoPlayer";


		private void Awake()
		{
			explorer.OnPathSelected += LoadVideo;
		}

		private void LoadVideo(string path)
		{
			LoadPath.Value = path;
			SceneManager.LoadScene(VideoPlayerSceneName);
		}
	}
}
