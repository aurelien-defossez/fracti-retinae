using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FractiRetinae
{
	public class MadameNature : MonoBehaviourSingleton<MadameNature>
	{
		[SerializeField] private GameObject ambiance1Land, ambiance2Land;
		[SerializeField] private Material ambiance1Skybox, ambiance2Skybox;
		[SerializeField] private Color ambiance1FogColor, ambiance2FogColor;
		[SerializeField] private Light mainLight;
		[SerializeField] private float targetIntensity;
		[SerializeField] private float blinkDuration;
		[SerializeField] private EaseDefinition fadeInEase, fadeOutEase, blinkInEase, blinkOutEase;

		private float startIntensity;

		protected override void Awake()
		{
			base.Awake();

			startIntensity = mainLight.intensity;
		}

		public void OnLevelStart()
		{
			mainLight.intensity = startIntensity;
			ambiance1Land.SetActive(true);
			ambiance2Land.SetActive(false);
			RenderSettings.skybox = ambiance1Skybox;
			RenderSettings.fogColor = ambiance1FogColor;
			DynamicGI.UpdateEnvironment();

			StartCoroutine(FadeIn());
		}

		public void OnGoalFound() => StartCoroutine(OnGoalFoundCore());

		private IEnumerator OnGoalFoundCore()
		{
			yield return Blink(startIntensity, 0, blinkInEase);
			yield return new WaitForSeconds(blinkDuration);
			ambiance1Land.SetActive(false);
			ambiance2Land.SetActive(true);
			RenderSettings.skybox = ambiance2Skybox;
			RenderSettings.fogColor = ambiance2FogColor;
			DynamicGI.UpdateEnvironment();
			yield return Blink(0, startIntensity, blinkOutEase);
		}

		public IEnumerator FadeIn() => Auto.Interpolate(targetIntensity, startIntensity, fadeInEase, i => mainLight.intensity = i);
		public IEnumerator FadeOut() => Auto.Interpolate(startIntensity, targetIntensity, fadeOutEase, i => mainLight.intensity = i);
		public IEnumerator Blink(float from, float to, EaseDefinition ease) => Auto.Interpolate(from, to, ease, i => mainLight.intensity = i);
	}
}
