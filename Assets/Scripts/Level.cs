using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FractiRetinae
{
	public class Level : MonoBehaviour
	{
		[SerializeField, Range(1, 9)] private int cameraCount = 2;
		[SerializeField] private Transform startPosition;
		[SerializeField] private EaseDefinition lookAtEase;

		public int CameraCount => cameraCount;
		public Transform Start => startPosition;

		private Glyph[] glyphs;

		protected void Awake()
		{
			glyphs = GetComponentsInChildren<Glyph>(includeInactive: true);
		}

		public void Load()
		{
			gameObject.SetActive(true);
			EnableGlyphs(Cheater.Instance.EnableGlyphsOnLoad);
			ScreenLayout.Instance.Setup(cameraCount);
			MusicManager.Instance.OnLevelStart();
			MadameNature.Instance.OnLevelStart();
			PlayerController.Instance.Controls.Player.Enable();

			if (Cheater.Instance.NoClip)
			{
				foreach(BoxCollider box in GetComponentsInChildren<BoxCollider>().Where(b => !b.isTrigger))
				{
					box.enabled = false;
				}
			}
		}

		public void OnGoalFound()
		{
			EnableGlyphs(true);
			MusicManager.Instance.OnGoalFound();
			MadameNature.Instance.OnGoalFound();
		}

		private void EnableGlyphs(bool enabled)
		{
			foreach (Glyph glyph in glyphs)
			{
				glyph.gameObject.SetActive(enabled);
			}

			if (enabled)
			{
				if (glyphs.Length == 0)
				{
					Debug.LogError($"There are no glyphs in this level, can't be finished.");
				}
				else
				{
					StartCoroutine(CheckGlyphDistance());
				}
			}
		}

		private IEnumerator CheckGlyphDistance()
		{
			while (gameObject.activeSelf)
			{
				if (!Cheater.Instance.DisableLevelEnd
				 && glyphs.All(g => g.CenterDistance <= LevelLoader.Instance.MaximalGlyphDistance
					&& g.NormalDifference <= LevelLoader.Instance.MaximalGlyphNormalDifference))
				{
					PlayerController.Instance.Controls.Player.Disable();
					yield return PlayerController.Instance.LookAt(glyphs.First().transform.position, lookAtEase);
					yield return MadameNature.Instance.FlashIntensity();
					LevelLoader.Instance.LoadNextLevel();
					break;
				}
				else
				{
					yield return 0;
				}
			}
		}
	}
}
