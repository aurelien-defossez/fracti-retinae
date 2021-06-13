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
		[SerializeField] public bool EnableGlyphsOnLoad;
		[SerializeField] public bool MuteMusic;
		[SerializeField, Min(1)] public int StartLevel = 1;

		protected void Start()
		{
			PlayerController.Instance.Controls.Cheater.NextLevel.performed += LoadNextLevel;

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
	}
}
