using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FractiRetinae
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField, Min(0)] private float walkSpeed;

		private PlayerControls controls;
		private Vector2 movementDirection;

		protected void Awake()
		{
			controls = new PlayerControls();

			controls.Player.Enable();
		}

		protected void Update()
		{
			movementDirection = controls.Player.Move.ReadValue<Vector2>();
			transform.position = transform.position + new Vector3(movementDirection.x, 0, movementDirection.y) * Time.deltaTime * walkSpeed;
		}
	}
}
