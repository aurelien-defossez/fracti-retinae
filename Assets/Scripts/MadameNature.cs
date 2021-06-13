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

		public void OnLevelStart()
		{
			ambiance1Land.SetActive(true);
			ambiance2Land.SetActive(false);
		}

		public void OnGoalFound()
		{
			ambiance1Land.SetActive(false);
			ambiance2Land.SetActive(true);
		}
	}
}
