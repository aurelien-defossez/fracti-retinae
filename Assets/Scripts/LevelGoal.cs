using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FractiRetinae
{
	public class LevelGoal : MonoBehaviour
	{
		protected void OnTriggerEnter(Collider collider)
		{
			if (collider.CompareTag("Player"))
			{
				StartCoroutine(LevelLoader.Instance.CurrentLevel.OnGoalFound());
			}
		}
	}
}
