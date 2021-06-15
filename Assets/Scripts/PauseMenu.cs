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

		protected void Start()
		{
			PlayerController.Instance.Controls.Player.Pause.performed += OnPause;

			UpdatePauseStatus();
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
	}
}
