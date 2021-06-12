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
		[SerializeField] public bool EnableGlyphsOnLevelLoad;

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
