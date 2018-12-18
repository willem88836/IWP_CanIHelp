using UnityEngine;

namespace IWPCIH.Explorer
{
	/// <summary>
	///		A 3D implementation of the Explorer.
	/// </summary>
	public class Explorer3D : Explorer
	{
		public TextMesh PathField;

		/// <summary>
		///		Updates the textfields to match the path.
		/// </summary>
		private void SetPathVisually(string path)
		{
			PathField.text = path;
		}

		/// <inheritdoc />
		public override void OnObjectSelected(string path)
		{
			base.OnObjectSelected(path);
			SetPathVisually(path);
		}
	}
}
