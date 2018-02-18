
using UnityEngine;

namespace Assets.Code.Menus
{
	public partial class UIManager
	{
		public static Transform Canvas { get; private set; }

		private BuildMenu _build;
		//private UpgradeMenu _upgrade;

		public bool InMainMenu { get { return _build != null && _build.Showing; } }

		public UIManager () {
			Canvas = GameObject.Find("Canvas").transform; // There should only ever be one canvas
		}

		public void ShowBuildMenu () {
			_build = new BuildMenu();
			_build.Show();
			//Debug.Log("1");
		}

		public void HideBuildMenu () {
			_build.Hide();
			_build = null;
		}



		private abstract class Menu
		{
			protected GameObject Go;
			public bool Showing { get; private set; }

			/// <summary>
			/// Show this menu
			/// </summary>
			public virtual void Show () {
				Showing = true;
				Go.SetActive(true);
			}

			/// <summary>
			/// Hide this menu
			/// </summary>
			public virtual void Hide () {
				GameObject.Destroy(Go);
				Showing = false;
			}
		}
	}

}