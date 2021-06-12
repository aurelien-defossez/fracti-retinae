﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FractiRetinae
{
	public class LevelLoader : MonoBehaviourSingleton<LevelLoader>
	{
		[SerializeField, Min(1)] private int startLevel = 1;

		public Level CurrentLevel => levels[levelIndex];

		private Level[] levels;
		private int levelIndex;

		protected void Awake()
		{
			levels = GetComponentsInChildren<Level>(includeInactive: true);
		}

		protected void Start()
		{
			LoadLevel(startLevel - 1);
		}

		public void LoadLevel(int index)
		{
			foreach (Level level in levels)
			{
				level.gameObject.SetActive(false);
			}

			levelIndex = index;
			levels[levelIndex].gameObject.SetActive(true);
			PlayerController.Instance.TeleportPlayer(CurrentLevel.Start.position, CurrentLevel.Start.rotation);
		}

		public void LoadNextLevel() => LoadLevel(levelIndex < levels.Length - 1 ? levelIndex + 1 : 0);
	}
}
