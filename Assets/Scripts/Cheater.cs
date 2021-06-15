using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FractiRetinae
{
	public class Cheater : MonoBehaviourSingleton<Cheater>
	{
		[SerializeField] public bool HotkeysEnabled;
		[SerializeField] public bool DisableLevelEnd;
		[SerializeField] public bool NoClip;
		[SerializeField] public bool MuteMusic;
		[SerializeField, Min(1)] public int StartLevel = 1;

		protected void Start()
		{
			PlayerController.Instance.Controls.Cheater.NextLevel.performed += LoadNextLevel;
			PlayerController.Instance.Controls.Cheater.ShowGlyphs.performed += ShowGlyphs;
			PlayerController.Instance.Controls.Cheater.EndLevel.performed += EndLevel;

			if (HotkeysEnabled)
			{
				PlayerController.Instance.Controls.Cheater.Enable();
			}
			else
			{
				PlayerController.Instance.Controls.Cheater.Disable();
			}
		}

		private void LoadNextLevel(InputAction.CallbackContext obj) => LevelLoader.Instance.LoadNextLevel();
		private void ShowGlyphs(InputAction.CallbackContext obj) => StartCoroutine(LevelLoader.Instance.CurrentLevel.OnGoalFound());
		private void EndLevel(InputAction.CallbackContext obj) => StartCoroutine(LevelLoader.Instance.CurrentLevel.EndLevel());
	}
}
