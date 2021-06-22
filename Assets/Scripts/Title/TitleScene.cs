using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FractiRetinae.Title
{
	public class TitleScene : MonoBehaviour
	{
		[SerializeField] GameObject quitButton;

		protected void Awake()
		{
			quitButton.SetActive(Application.platform != RuntimePlatform.WebGLPlayer);

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		public void PlayGame() => SceneManager.LoadScene("Game");

		public void QuitGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
