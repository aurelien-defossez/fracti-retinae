using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FractiRetinae
{
	public class LevelGoal : MonoBehaviour
	{
		protected void OnCollisionEnter(Collision collision)
		{
			if (collision.collider.CompareTag("Player"))
			{
				GetComponentInParent<LevelLoader>().LoadNextLevel();
			}
		}
	}
}
