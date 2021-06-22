using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FractiRetinae
{
	public class PauseMenu : MonoBehaviourSingleton<PauseMenu>
	{
		[SerializeField] private AudioMixer mixer;
		[SerializeField] private GameObject window;
		[SerializeField] private Slider volume;
		[SerializeField] private Slider sensitivity;

		public bool IsPaused { get; set; }

		private bool wasFullScreen;

		protected void Start()
		{
			PlayerController.Instance.Controls.Player.Pause.performed += OnPause;

			wasFullScreen = Screen.fullScreen;

			UpdatePauseStatus();
		}

		protected void Update()
		{
			if (wasFullScreen != Screen.fullScreen)
			{
				// Open the pause when going out of full screen mode. Similarly, close it when going full screen.
				// This is useful for Chrome, where Escape quits full screen mode without bubbling the key event to us.
				IsPaused = !Screen.fullScreen;
				wasFullScreen = Screen.fullScreen;
				UpdatePauseStatus();
			}
			else if (Application.platform == RuntimePlatform.WebGLPlayer && !Screen.fullScreen && !IsMouseInsideScreen() && !IsPaused)
			{
				// Open the pause menu if mouse leaves the screen in open GL, it means the user pressed escape, and got his mouse back.
				IsPaused = true;
				UpdatePauseStatus();
			}
		}

		private void OnPause(InputAction.CallbackContext context)
		{
			IsPaused = !IsPaused;
			UpdatePauseStatus();
		}

		public void UpdateVolume() => mixer.SetFloat("MainVolume", Mathf.Lerp(-80, 0, Mathf.Pow(volume.value, .25f)));

		public void UpdateSensitivity() => PlayerController.Instance.SensitivitySetting = sensitivity.value;

		private void UpdatePauseStatus()
		{
			Time.timeScale = IsPaused ? 0 : 1;
			window.SetActive(IsPaused);
			Cursor.lockState = IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = IsPaused;
		}

		private bool IsMouseInsideScreen()
		{
			Vector2 topRight = Camera.main.ViewportToScreenPoint(Vector2.one);
			Vector2 mouse = Mouse.current.position.ReadValue();

			return mouse.x > 0 && mouse.x < topRight.x && mouse.y > 0 && mouse.y < topRight.y;
		}
	}
}
