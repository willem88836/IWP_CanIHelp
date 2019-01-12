using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace IWPCIH.Video
{
	[RequireComponent(typeof(VideoPlayer))]
	public class VideoThumbnail : MonoBehaviour
	{
		public enum ResizeMode { Horizontal, Vertical, None }

		private static VideoThumbnail Instance;
		public VideoPlayer thumbnailVideoPlayer;
		[Range(0, 1)] public float ThumbnailFrame;
		

		private void Awake()
		{
			if (Instance != null)
			{
				Debug.LogWarning("Can't have two VideoThumbnail instances running at the same time!\nInitial instance is now obsolete.");
			}

			if (thumbnailVideoPlayer == null)
				thumbnailVideoPlayer = GetComponent<VideoPlayer>();

			Instance = this;
		}

		public static void SetThumbnail(string path, RawImage target, ResizeMode mode = ResizeMode.None)
		{
			if (Instance == null)
			{
				Debug.LogWarning("No VideoThumbnail instance active!");
				return;
			}

			Instance.StartCoroutine(CreateThumbnail(path, target, mode));
		}

		private static IEnumerator CreateThumbnail(string path, RawImage target, ResizeMode mode)
		{
			VideoPlayer player = Instance.thumbnailVideoPlayer;

			player.url = path;
			player.Prepare();

			// TODO: swap this with the prepComplete in VideoPlayer.
			while(!player.isPrepared)
			{
				yield return new WaitForEndOfFrame();
			}

			player.targetTexture = new RenderTexture((RenderTexture)player.texture);

			player.time = (player.frameCount / player.frameRate) * Instance.ThumbnailFrame;

			player.Play();
			yield return new WaitForEndOfFrame();


			target.texture = player.targetTexture;

			int videoWidth = player.targetTexture.width;
			int videoHeight = player.targetTexture.height;
			switch (mode)
			{
				case ResizeMode.None:
					target.rectTransform.sizeDelta = new Vector2(videoWidth, videoHeight);
					break;
				case ResizeMode.Horizontal:
					float deltaX = target.rectTransform.sizeDelta.x;
					target.rectTransform.sizeDelta = new Vector2(deltaX, videoHeight * (deltaX / videoWidth));
					break;
				case ResizeMode.Vertical:
					float deltaY = target.rectTransform.sizeDelta.y;
					target.rectTransform.sizeDelta = new Vector2(videoWidth * (deltaY / videoHeight), deltaY);
					break;
			}

			player.Pause();
		}
	}
}
