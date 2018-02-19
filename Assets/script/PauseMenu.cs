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
		private class PauseMenu : Menu
		{
			
			public PauseMenu()
			{
				
				Go = (GameObject)Object.Instantiate(Resources.Load("Pause Menu"),Canvas);
				InitializeButtons();
				

			}

			private void InitializeButtons()
			{
				var _resume = GameObject.Find("Resume").GetComponent<Button>();
				var _restart = GameObject.Find("Restart").GetComponent<Button>();
				var _quit = GameObject.Find("Quit").GetComponent<Button>();
				

				if (_resume !=null || _restart!=null || _quit!=null)
				{				
					Time.timeScale=0;
				}

				_resume.onClick.AddListener(() =>
				{
					Time.timeScale=1;
					
					Hide();				
				});
				
				
				_restart.onClick.AddListener(() =>
				{				
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



