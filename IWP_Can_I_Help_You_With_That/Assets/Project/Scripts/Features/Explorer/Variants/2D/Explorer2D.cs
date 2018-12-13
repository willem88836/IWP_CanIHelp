﻿using UnityEngine.UI;

namespace IWPCIH.ExplorerII
{
	/// <summary>
	///		A 2D implementation of the Explorer.
	/// </summary>
	public sealed class Explorer2D : Explorer
	{
		public Text SelectedPath;
		public Button SelectedPathConfirmationButton;
		public InputField PathInputField;
		public Button PathInputFieldConfirmationButton;


		/// <inheritdoc />
		public override void OnEnable()
		{
			base.OnEnable();

			SelectedPath.text = PathInputField.text = ExplorerPath.Value;

			SelectedPathConfirmationButton.onClick.AddListener(ConfirmSelectedPath);
			PathInputFieldConfirmationButton.onClick.AddListener(delegate
			{
				OnObjectSelected(PathInputField.text);
			});

			SetPathVisually(ExplorerPath.Value);
		}

		/// <summary>
		///		Updates the textfields to match the path.
		/// </summary>
		private void SetPathVisually(string path)
		{
			SelectedPath.text = path;
			PathInputField.text = path;
		}

		/// <inheritdoc />
		public override void OnObjectSelected(string path)
		{
			base.OnObjectSelected(path);
			SetPathVisually(path);
		}
	}
}