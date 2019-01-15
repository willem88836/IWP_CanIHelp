#if !DISABLE_SCREENSHOTS
// ty 8D

using UnityEngine;
using System.IO;

namespace Framework.Features.DebugTools
{
	[RequireComponent(typeof(Camera))]
	public class Screenshot : MonoBehaviour
	{
		[SerializeField] KeyCode _button = KeyCode.F12;
		[SerializeField] int _width;
		[SerializeField] int _height;
		[SerializeField] string _path = "";
		[SerializeField] string _name = "ScreenShot_";
		[SerializeField] int _count;

		const string StartPath = "Assets/";

		string Path { get { return StartPath + _path + _name + (_count++).ToString("00") + ".png"; } }

		void Update()
		{
			if (Input.GetKeyDown(_button)) { Shoot(); }
		}

		void Shoot()
		{
			Camera original = GetComponent<Camera>();
			RenderTexture tmpTexture = original.targetTexture;
			int width = _width < 1 ? original.pixelWidth : _width;
			int height = _height < 1 ? original.pixelHeight : _height;

			RenderTexture tex = new RenderTexture(width, height, 24);
			Texture2D shot = new Texture2D(width, height, TextureFormat.RGB24, false);
			original.targetTexture = tex;
			original.Render();

			RenderTexture.active = tex;
			shot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			byte[] bytes = shot.EncodeToPNG();
			File.WriteAllBytes(Path, bytes);

			RenderTexture.active = null;
			original.targetTexture = tmpTexture;
			Destroy(tex);

			#if UNITY_EDITOR
			UnityEditor.AssetDatabase.Refresh();
			#endif

			Debug.Log("<color=blue>Screenshot: </color>Screenshot taken!");
		}
	}
}

#endif