using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;


namespace Assets.Code.Menus
{
	public partial class UIManager
	{
		private class BuildMenu : Menu
		{
			
			public BuildMenu()
			{

				
				
				Go = (GameObject)Object.Instantiate(Resources.Load("Build Menu"),Canvas);
				InitializeButtons();
				

			}

			private void InitializeButtons()
			{
				var _restart = GameObject.Find("Restart").GetComponent<Button>();
				var _quit = GameObject.Find("Quit").GetComponent<Button>();

				if (_restart!=null || _quit!=null)
				{				
					Time.timeScale=0;
				}

				_restart.onClick.AddListener(() =>
				{
					Debug.Log("restart");				
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					Time.timeScale=1;			
					Hide();				
				});


				
				_quit.onClick.AddListener(() =>
				{
					Debug.Log("quit");
					
					Application.Quit();
					UnityEditor.EditorApplication.isPlaying = false;
			
				});
				
				

			}

			
			

		}	
	}
}


