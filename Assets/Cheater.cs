using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FractiRetinae
{
	public class Cheater : MonoBehaviour
	{
		[SerializeField] private bool hotkeysEnabled;
		[SerializeField] private PlayerController controller;
		[SerializeField] private LevelLoader levelLoader;

		protected void Start()
		{
			controller.Controls.Cheater.NextLevel.performed += LoadNextLevel;

			if (hotkeysEnabled)
			{
				controller.Controls.Cheater.Enable();
			}
			else
			{
				controller.Controls.Cheater.Disable();
			}
		}

		private void LoadNextLevel(InputAction.CallbackContext obj) => levelLoader.LoadNextLevel();
	}
}
