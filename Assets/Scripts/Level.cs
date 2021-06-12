using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FractiRetinae
{
	public class Level: MonoBehaviour
	{
		[SerializeField, Range(1, 9)] private int cameraCount = 2;
		[SerializeField] private Transform startPosition;

		public Vector3 StartPosition => startPosition.position;
	}
}
