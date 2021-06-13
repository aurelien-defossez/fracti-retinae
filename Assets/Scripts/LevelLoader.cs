using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FractiRetinae
{
	public class LevelLoader : MonoBehaviourSingleton<LevelLoader>
	{
		[SerializeField, Range(0, 1)] private float maximalGlyphDistance;

		public float MaximalGlyphDistance => maximalGlyphDistance;
		public Level CurrentLevel => levels[levelIndex];
		public MusicManager musicManager;

		private Level[] levels;
		private int levelIndex;

		protected override void Awake()
		{
			base.Awake();

			levels = GetComponentsInChildren<Level>(includeInactive: true);
		}

		protected void Start()
		{
			LoadLevel(Cheater.Instance.StartLevel - 1);
		}

		public void LoadLevel(int index)
		{
			foreach (Level level in levels)
			{
				level.gameObject.SetActive(false);
			}

			Debug.Log($"Load level #{index + 1}");

			levelIndex = index;
			CurrentLevel.Load();
			PlayerController.Instance.TeleportPlayer(CurrentLevel.Start.position, CurrentLevel.Start.rotation);

			musicManager.OnLevelStart();
		}

		public void LoadNextLevel() => LoadLevel(levelIndex < levels.Length - 1 ? levelIndex + 1 : 0);
	}
}
