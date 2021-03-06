﻿using System;
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
				var _resume = GameObject.FindGameObjectWithTag("resume").GetComponent<Button>();
				var _instruction = GameObject.FindGameObjectWithTag("instruction").GetComponent<Button>();
				var _restart = GameObject.FindGameObjectWithTag("restart").GetComponent<Button>();
				var _quit = GameObject.FindGameObjectWithTag("quit").GetComponent<Button>();
				//var _resume = GameObject.Find("Resume").GetComponent<Button>();
				//var _restart = GameObject.Find("Restart").GetComponent<Button>();
				//var _quit = GameObject.Find("Quit").GetComponent<Button>();
				GameObject mainCharater = GameObject.FindGameObjectWithTag("Player");
				MainCharacter mc = mainCharater.GetComponent<MainCharacter>();

				

				if (_resume !=null || _restart!=null || _quit!=null || _instruction!=null)
				{
					Time.timeScale = 0;
					/*
					Object[] objects = GameObject.FindObjectsOfType (typeof(GameObject));
					foreach (GameObject go in objects) {
						go.SendMessage ("OnPauseGame", SendMessageOptions.DontRequireReceiver);
					}
					*/
				}

				_resume.onClick.AddListener(() =>
				{
					Time.timeScale=1;
					
					
					/*
					Object[] objects = GameObject.FindObjectsOfType (typeof(GameObject));
					foreach (GameObject go in objects) {
						go.SendMessage ("OnResumeGame", SendMessageOptions.DontRequireReceiver);
					}
					//GameObject.Destroy(Go,0);
					*/
					mc.menushowed = false;
					Hide();				
				});
				
				_instruction.onClick.AddListener(() =>
				{
					SceneManager.LoadScene(3);
					Time.timeScale=1;
					/*
					Object[] objects = GameObject.FindObjectsOfType (typeof(GameObject));
					foreach (GameObject go in objects) {
						go.SendMessage ("OnResumeGame", SendMessageOptions.DontRequireReceiver);
					}
					//GameObject.Destroy(Go,0);
					*/
					
					Hide();				
				});
				
				
				_restart.onClick.AddListener(() =>
				{				
					//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					SceneManager.LoadScene(0);
					Time.timeScale=1;			
					Hide();				
				});


				
				_quit.onClick.AddListener(() =>
				{
					//Debug.Log("quit");
					
					Application.Quit();
					//UnityEditor.EditorApplication.isPlaying = false;
			
				});
				
				

			}

			
			

		}	
	}
}


