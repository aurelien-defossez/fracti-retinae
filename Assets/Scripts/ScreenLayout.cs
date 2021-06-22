using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FractiRetinae
{
	public class ScreenLayout : MonoBehaviourSingleton<ScreenLayout>
	{
		[SerializeField] private MeshRenderer[] screens;
		[SerializeField] private EaseDefinition shatterEase, rejoinEase;
		[SerializeField] private CameraShake cameraShake;
		[SerializeField] private string shatterText;
		[SerializeField] private AudioSource shatterSound, shatterSoundLight;
		[SerializeField] private float flickerDuration;

		private float xLeft, xRight, yTop, yBottom;

		protected override void Awake()
		{
			base.Awake();

			xLeft = screens[0].transform.localPosition.x;
			xRight = screens[1].transform.localPosition.x;
			yTop = screens[0].transform.localPosition.y;
			yBottom = screens[2].transform.localPosition.y;
		}

		public void Setup(int screenCount)
		{
			for (int i = 0; i < screens.Length; i++)
			{
				screens[i].gameObject.SetActive(i < screenCount);
			}

			// X position of 1st screen
			if (screenCount == 1)
			{
				screens[0].transform.localPosition = Vector3.zero;
				screens[1].transform.localPosition = Vector3.zero;
			}
			else if (screenCount == 2)
			{
				screens[0].transform.localPosition = new Vector3(xLeft, 0, 0);
				screens[1].transform.localPosition = new Vector3(xRight, 0, 0);
			}
			else
			{
				screens[0].transform.localPosition = new Vector3(xLeft, yTop, 0);
				screens[1].transform.localPosition = new Vector3(xRight, yTop, 0);
				screens[2].transform.localPosition = screenCount == 3 ? new Vector3(0, yBottom, 0) : new Vector3(xLeft, yBottom, 0);
			}
		}

		public void PlayerLightShatterSound() => shatterSoundLight.Play();

		public IEnumerator ShatterScreens()
		{
			screens[1].gameObject.SetActive(true);

			cameraShake.MinTrauma = 1;
			TextPrinter.Instance.PrintText(shatterText);
			shatterSound.Play();

			//Flicker
			float flickerStartTime = Time.time;
			while (Time.time-flickerStartTime < flickerDuration)
			{
				screens[0].transform.localPosition = screens[0].transform.localPosition.WithZ(-0.1f);
				screens[1].transform.localPosition = screens[1].transform.localPosition.WithZ(0.1f);
				yield return 0;
				screens[0].transform.localPosition = screens[0].transform.localPosition.WithZ(0.1f);
				screens[1].transform.localPosition = screens[1].transform.localPosition.WithZ(-0.1f);
				yield return 0;
			}

			screens[0].transform.localPosition = screens[0].transform.localPosition.WithZ(0);
			screens[1].transform.localPosition = screens[1].transform.localPosition.WithZ(0);

			// Move apart
			yield return Auto.Interpolate(0, xRight, shatterEase, x =>
			{
				screens[0].transform.localPosition = screens[0].transform.localPosition.WithX(-x);
				screens[1].transform.localPosition = screens[1].transform.localPosition.WithX(x);
			});

			TextPrinter.Instance.HideText();
			cameraShake.MinTrauma = 0;
		}

		public IEnumerator RejoinScreens()
		{
			cameraShake.MinTrauma = 0.5f;

			yield return Auto.Interpolate(0, 1, rejoinEase, t =>
			{
				screens[0].transform.localPosition = Vector3.Lerp(new Vector3(xLeft, yTop, 0), Vector3.zero, t);
				screens[1].transform.localPosition = Vector3.Lerp(new Vector3(xRight, yTop, 0), Vector3.zero, t);
				screens[2].transform.localPosition = Vector3.Lerp(new Vector3(0, yBottom, 0), Vector3.zero, t);
			});

			cameraShake.MinTrauma = 0;
			TextPrinter.Instance.PrintText("Your soul is rejoined");
		}
	}
}
