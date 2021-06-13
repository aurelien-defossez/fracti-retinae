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

		public void OnLevelStart()
		{
			ambiance1Land.SetActive(true);
			ambiance2Land.SetActive(false);
			RenderSettings.skybox = ambiance1Skybox;
			RenderSettings.fogColor = ambiance1FogColor;
			DynamicGI.UpdateEnvironment();
		}

		public void OnGoalFound()
		{
			ambiance1Land.SetActive(false);
			ambiance2Land.SetActive(true);
			RenderSettings.skybox = ambiance2Skybox;
			RenderSettings.fogColor = ambiance2FogColor;
			DynamicGI.UpdateEnvironment();
		}
	}
}
