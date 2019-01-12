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

			//if (thumbnailVideoPlayer.targetTexture == null)
			//	thumbnailVideoPlayer.targetTexture = new RenderTexture(1, 1, 0);

			Instance = this;
		}

		public static void SetThumbnail(string path, RawImage target, ResizeMode mode = ResizeMode.None)
		{
			if (Instance == null)
			{
				Debug.LogWarning("No VideoThumbnail instance active!");
				return;
			}

			VideoPlayer player = Instance.thumbnailVideoPlayer;

			player.targetTexture = new RenderTexture(1, 1, 0);

			player.url = path;
			player.prepareCompleted += (VideoPlayer vp) => { Instance.StartCoroutine(OnPlayerPrepared(vp, target, mode)); };
			player.Prepare();
		}

		private static IEnumerator OnPlayerPrepared(VideoPlayer player, RawImage target, ResizeMode mode)
		{
			player.time = (player.frameCount / player.frameRate) * Instance.ThumbnailFrame;

			player.targetTexture.width = player.texture.width;
			player.targetTexture.height = player.texture.height;

			player.Play();

			yield return new WaitForSeconds(3);

			player.Pause();

			RenderTexture tn = new RenderTexture(player.targetTexture.width, player.targetTexture.height, player.targetTexture.depth);
			Graphics.CopyTexture(player.targetTexture, tn);
			target.texture = tn;

			int videoWidth = target.texture.width;
			int videoHeight = target.texture.height;
			if (mode == ResizeMode.Horizontal)
			{
				float deltaX = target.rectTransform.sizeDelta.x;
				target.rectTransform.sizeDelta = new Vector2(deltaX, videoHeight * (deltaX / videoWidth));
			}
			else if (mode == ResizeMode.Vertical)
			{
				float deltaY = target.rectTransform.sizeDelta.y;
				target.rectTransform.sizeDelta = new Vector2(videoWidth * (deltaY / videoHeight), deltaY);
			}
			else
			{
				target.rectTransform.sizeDelta = new Vector2(videoWidth, videoHeight);
			}

			player.Stop();
		}
	}
}
