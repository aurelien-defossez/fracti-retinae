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

		public Transform Start => startPosition;

		private Glyph[] glyphs;

		protected void Awake()
		{
			glyphs = GetComponentsInChildren<Glyph>(includeInactive: true);
		}

		public void Load()
		{
			gameObject.SetActive(true);
			EnableGlyphs(Cheater.Instance.EnableGlyphsOnLevelLoad);
			ScreenLayout.Instance.Setup(cameraCount);
		}

		public void EnableGlyphs(bool enabled)
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
				if (glyphs.All(g => g.CenterDistance <= LevelLoader.Instance.MaximalGlyphDistance))
				{
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
