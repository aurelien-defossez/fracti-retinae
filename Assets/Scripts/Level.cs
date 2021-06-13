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
		[SerializeField] private LayerMask allViewsMask;

		public int CameraCount => cameraCount;
		public Transform Start => startPosition;

		private Glyph[] glyphs;
		private bool isSearchingForGlyphs;
		private int initialCameraCullingMask;

		protected void Awake()
		{
			glyphs = GetComponentsInChildren<Glyph>(includeInactive: true);
		}

		public void Load()
		{
			isSearchingForGlyphs = false;
			gameObject.SetActive(true);
			ScreenLayout.Instance.Setup(cameraCount);
			MusicManager.Instance.OnLevelStart();
			MadameNature.Instance.OnLevelStart();
			PlayerController.Instance.Controls.Player.Enable();
			PlayerController.Instance.CameraShake.MinTrauma = 0;

			if (cameraCount == 1)
			{
				initialCameraCullingMask = PlayerController.Instance.Cameras[0].cullingMask;
				PlayerController.Instance.Cameras[0].cullingMask = allViewsMask;
			}

			foreach (Glyph glyph in glyphs)
			{
				glyph.gameObject.SetActive(false);
			}

			if (Cheater.Instance.NoClip)
			{
				foreach (BoxCollider box in GetComponentsInChildren<BoxCollider>().Where(b => !b.isTrigger))
				{
					box.enabled = false;
				}
			}
		}

		public void OnGoalFound()
		{
			if (!isSearchingForGlyphs)
			{
				isSearchingForGlyphs = true;
				MusicManager.Instance.OnGoalFound();
				MadameNature.Instance.OnGoalFound();

				foreach (Glyph glyph in glyphs)
				{
					glyph.ShowGlyph();
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

				// First level: Shatter thy soul!
				if (CameraCount == 1)
				{
					PlayerController.Instance.Cameras[0].cullingMask = initialCameraCullingMask;
					StartCoroutine(ShatterSouls());
				}
			}
		}

		private IEnumerator ShatterSouls()
		{
			PlayerController.Instance.Controls.Player.Disable();
			yield return ScreenLayout.Instance.ShatterScreens();
			PlayerController.Instance.Controls.Player.Enable();
		}

		private IEnumerator CheckGlyphDistance()
		{
			while (gameObject.activeSelf)
			{
				int activatedGlyphs = glyphs.Count(g => g.CenterDistance <= LevelLoader.Instance.MaximalGlyphDistance
					&& g.NormalDifference <= LevelLoader.Instance.MaximalGlyphNormalDifference);

				PlayerController.Instance.CameraShake.MinTrauma = activatedGlyphs * 1f / glyphs.Length;

				if (!Cheater.Instance.DisableLevelEnd && activatedGlyphs == glyphs.Length)
				{
					PlayerController.Instance.Controls.Player.Disable();
					yield return PlayerController.Instance.LookAt(glyphs.First().transform.position, lookAtEase);
					yield return MadameNature.Instance.FadeOut();
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
