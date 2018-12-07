using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
	[RequireComponent(typeof(RawImage))]
	[RequireComponent(typeof(RectTransform))]
	public class LineRenderer2DController : MonoBehaviour
	{
		public static LineRenderer2DController Instance;

		public ComputeShader LineRenderShader;

		private RenderTexture targetTexture;
		private List<LineRenderer2D> lineRenderer2Ds = new List<LineRenderer2D>();


		private void Awake()
		{
			if (Instance != null)
				Destroy(Instance.gameObject);
			Instance = this;


			Rect rect = GetComponent<RectTransform>().rect;
			targetTexture = new RenderTexture((int)(rect.width), (int)(rect.height), 24);
			targetTexture.name = "LineRenderer2DController_Output";
			targetTexture.enableRandomWrite = true;
			targetTexture.Create();

			RawImage rawImage = GetComponent<RawImage>();
			rawImage.texture = targetTexture;
			// Not necessary. However, now the default scene image doesn't have to be bright white. 
			rawImage.color = Color.white; 
		}

		private void Update()
		{
			if (!targetTexture || !LineRenderShader)
				return;


			int threadGroupsX = targetTexture.width / 8;
			int threadGroupsY = targetTexture.height / 8;

			// Clears render image.
			int clear = LineRenderShader.FindKernel("Clear");
			LineRenderShader.SetTexture(clear, "Result", targetTexture);
			LineRenderShader.Dispatch(clear, threadGroupsX, threadGroupsY, 1);

			// Renders lines on renderimage.
			int kernel = LineRenderShader.FindKernel("CSMain");
			LineRenderShader.SetTexture(kernel, "Result", targetTexture);
			LineRenderShader.SetInt("Iterations", lineRenderer2Ds.Count);
			foreach (LineRenderer2D renderer in lineRenderer2Ds)
			{
				if (renderer.Path == null || renderer.Path.Length == 0)
					continue;

				// Draws one line onto the texture.
				LineRenderShader.SetInt("PathCount", renderer.Path.Length);
				LineRenderShader.SetVectorArray("Path", renderer.Path);
				LineRenderShader.SetVector("Color", renderer.Color);
				LineRenderShader.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);
			}
		}


		public void Add(LineRenderer2D renderer)
		{
			if (!lineRenderer2Ds.Contains(renderer))
				lineRenderer2Ds.Add(renderer);
		}

		public void Remove(LineRenderer2D renderer)
		{
			if (lineRenderer2Ds.Contains(renderer))
				lineRenderer2Ds.Remove(renderer);
		}
	}
}
