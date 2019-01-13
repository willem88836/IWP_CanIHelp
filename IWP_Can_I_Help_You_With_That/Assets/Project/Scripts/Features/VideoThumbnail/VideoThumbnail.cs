using Framework.Core;
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

		private const string TEXTURENAME = "thumbnail_target_texture";
		private const string THUMBNAILNAME = "thumbnail_{0}";

		private static VideoThumbnail instance;
		private static System.Action onVideoPlayerPrepared = null;

		public VideoPlayer ThumbnailVideoPlayer;
		[Range(0, 1)] public float ThumbnailFrame;


		private void Awake()
		{
			if (instance != null)
			{
				Debug.LogWarning("Can't have two VideoThumbnail instances running at the same time!\nInitial instance is now obsolete.");
			}

			if (ThumbnailVideoPlayer == null)
				ThumbnailVideoPlayer = GetComponent<VideoPlayer>();

			ThumbnailVideoPlayer.prepareCompleted += delegate { onVideoPlayerPrepared.SafeInvoke(); };

			instance = this;
		}

		public static void SetThumbnail(string path, RawImage target, ResizeMode mode = ResizeMode.None)
		{
			if (instance == null)
			{
				throw new MissingReferenceException("No VideoThumbnail instance active!"); 
			}

			VideoPlayer player = instance.ThumbnailVideoPlayer;

			player.targetTexture = new RenderTexture(1, 1, 0) { name = TEXTURENAME };
			player.url = path;

			onVideoPlayerPrepared = () => { instance.StartCoroutine(OnPlayerPrepared(player, target, mode)); };
			player.Prepare();
		}

		private static IEnumerator OnPlayerPrepared(VideoPlayer player, RawImage target, ResizeMode mode)
		{
			player.time = (player.frameCount / player.frameRate) * instance.ThumbnailFrame;

			player.targetTexture.width = player.texture.width;
			player.targetTexture.height = player.texture.height;

			player.Play();
			player.Pause();

			yield return new WaitForSeconds(3);


			target.texture = new RenderTexture(
				player.targetTexture.width, 
				player.targetTexture.height, 
				player.targetTexture.depth)
			{ name = string.Format(THUMBNAILNAME, Time.time.ToString()) };

			Graphics.CopyTexture(player.targetTexture, target.texture);

			Resize(target.texture, target.rectTransform, mode);
		}

		private static void Resize(Texture texture, RectTransform rect, ResizeMode mode)
		{
			int videoWidth = texture.width;
			int videoHeight = texture.height;
			if (mode == ResizeMode.Horizontal)
			{
				float deltaX = rect.sizeDelta.x;
				rect.sizeDelta = new Vector2(deltaX, videoHeight * (deltaX / videoWidth));
			}
			else if (mode == ResizeMode.Vertical)
			{
				float deltaY = rect.sizeDelta.y;
				rect.sizeDelta = new Vector2(videoWidth * (deltaY / videoHeight), deltaY);
			}
			else
			{
				rect.sizeDelta = new Vector2(videoWidth, videoHeight);
			}
		}
	}
}
