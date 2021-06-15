using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace FractiRetinae
{
	public class LevelLoader : MonoBehaviourSingleton<LevelLoader>
	{
		[SerializeField, Range(0, 1)] private float maximalGlyphDistance;
		[SerializeField, Range(0, 90)] private float maximalGlyphNormalDifference;
		[SerializeField] private string glyphTutorialMessage;

		public float MaximalGlyphDistance => maximalGlyphDistance;
		public float MaximalGlyphNormalDifference => maximalGlyphNormalDifference;
		public string GlyphTutorialMessage => glyphTutorialMessage;
		public Level CurrentLevel => levels[levelIndex];
		public bool HasMoreLevels => levelIndex < levels.Length - 1;

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
		}

		public void LoadNextLevel()
		{
			if (HasMoreLevels)
			{
				LoadLevel(levelIndex + 1);
			}
			else
			{
				StartCoroutine(ShowEndText());
			}
		}

		private IEnumerator ShowEndText()
		{
			yield return MusicManager.Instance.FadeMusicCore();
			yield return new WaitForSeconds(4);

			SceneManager.LoadScene("Title");
		}
	}
}
