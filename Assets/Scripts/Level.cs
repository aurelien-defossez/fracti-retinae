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
		[SerializeField] private EaseDefinition lookAtCthuluhuEase;
		[SerializeField] private EaseDefinition lookAtGlyphsEase;
		[SerializeField] private LayerMask allViewsMask;
		[SerializeField, Min(0)] private float glyphTutorialMessageDelay = 10;

		public int CameraCount => cameraCount;
		public Transform Start => startPosition;

		private Glyph[] glyphs;
		private bool isSearchingForGlyphs;
		private int initialCameraCullingMask;
		private Coroutine tutorialMessageRoutine;

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
			TextPrinter.Instance.HideText();

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

		public IEnumerator OnGoalFound()
		{
			if (!isSearchingForGlyphs)
			{
				isSearchingForGlyphs = true;

				// Recenter view
				PlayerController.Instance.Controls.Player.Disable();
				yield return PlayerController.Instance.LookAt(GetComponentInChildren<LevelGoal>().transform.position, lookAtCthuluhuEase, onlyHorizontal: true);
				PlayerController.Instance.Controls.Player.Enable();

				// Change scenery
				MusicManager.Instance.OnGoalFound();
				MadameNature.Instance.OnGoalFound();
				this.RestartCoroutine(ref tutorialMessageRoutine, ShowTutorialMessage());

				// Show glyphs
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
					PlayerController.Instance.Controls.Player.Disable();
					yield return ScreenLayout.Instance.ShatterScreens();
					PlayerController.Instance.Controls.Player.Enable();
				}
			}
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
					yield return EndLevel();
					break;
				}
				else
				{
					yield return 0;
				}
			}
		}

		public IEnumerator EndLevel()
		{
			PlayerController.Instance.CameraShake.MinTrauma = 1;
			this.TryStopCoroutine(ref tutorialMessageRoutine);
			TextPrinter.Instance.HideText();
			PlayerController.Instance.Controls.Player.Disable();
			yield return PlayerController.Instance.LookAt(glyphs.First().transform.position, lookAtGlyphsEase);

			if (!LevelLoader.Instance.HasMoreLevels)
			{
				yield return ScreenLayout.Instance.RejoinScreens();

				foreach (Camera camera in PlayerController.Instance.Cameras)
				{
					camera.cullingMask = allViewsMask;
				}
			}

			yield return MadameNature.Instance.FadeOut();
			yield return MusicManager.Instance.FadeGlyphsOut();
			LevelLoader.Instance.LoadNextLevel();
		}

		private IEnumerator ShowTutorialMessage()
		{
			yield return new WaitForSeconds(glyphTutorialMessageDelay);
			TextPrinter.Instance.PrintText(LevelLoader.Instance.GlyphTutorialMessage);
		}
	}
}
