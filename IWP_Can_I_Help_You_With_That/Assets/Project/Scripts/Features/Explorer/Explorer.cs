using System;
using System.Collections.Generic;
using UnityEngine;

namespace IWPCIH.Explorer
{
	public class Explorer : MonoBehaviour
	{
		[Serializable]
		public class ViewAction
		{
			public enum UpdateType { None, All, OnlySelf, AllExceptSelf }

			public ExplorerView View;
			public UpdateType Type = UpdateType.None;
		}

		public List<ViewAction> Actions;


		private void Start()
		{
			string path = "D:\\Users\\wille\\Documents\\Education\\Classes\\Y4B1_Prot\\Assignment_001\\";

			foreach (ViewAction action in Actions)
			{
				Update(action, path);
			}
		}

		private void OnObjectInvoke(ExplorerView parent, ExplorerViewObject invoked)
		{
			ViewAction va = Actions.Find((ViewAction v) => v.View == parent);

			if (va == null)
				return;

			Invoke(va);
		}

		private void Invoke(ViewAction va)
		{
			string path = va.View.Path;


			if (va.Type == ViewAction.UpdateType.None)
				return;

			if (va.Type == ViewAction.UpdateType.OnlySelf)
			{
				Update(va, path);
				return;
			}

			foreach (ViewAction action in Actions)
			{
				if (va.Type == ViewAction.UpdateType.AllExceptSelf && action == va)
					continue;

				Update(va, path);
			}
		}

		private void Update(ViewAction va, string path)
		{
			va.View.OnObjectInvoke += OnObjectInvoke;
			va.View.Initialize(path);
		}
	}
}