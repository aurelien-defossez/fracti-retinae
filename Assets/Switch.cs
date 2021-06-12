using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FractiRetinae
{
	public class Switch : MonoBehaviour
	{
		[SerializeField] private SlidingPlatform[] platforms;

		public void Activate()
		{
			Debug.Log($"Activate {name}");

			foreach (SlidingPlatform platform in platforms)
			{
				platform.Switch();
			}
		}
	}
}
