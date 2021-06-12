using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FractiRetinae
{
	public class LevelLoader : MonoBehaviour
	{
		[SerializeField] private PlayerController playerController;

		public Level CurrentLevel => levels[levelIndex];

		private Level[] levels;
		private int levelIndex;

		protected void Awake()
		{
			levels = GetComponentsInChildren<Level>(includeInactive: true);
		}

		protected void Start()
		{
			LoadLevel(0);
		}

		public void LoadLevel(int index)
		{
			foreach (Level level in levels)
			{
				level.gameObject.SetActive(false);
			}

			levelIndex = index;
			levels[levelIndex].gameObject.SetActive(true);
			playerController.transform.position = CurrentLevel.StartPosition;
		}

		public void LoadNextLevel() => LoadLevel(levelIndex < levels.Length - 1 ? levelIndex + 1 : 0);
	}
}
