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
		[SerializeField, Min(0)] private float walkSpeed = 1;
		[SerializeField, Min(0)] private float xSensitivity = 1;
		[SerializeField, Min(0)] private float ySensitivity = 1;

		private PlayerControls controls;

		protected void Awake()
		{
			controls = new PlayerControls();
		}

		protected void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			controls.Player.Enable();
		}

		protected void Update()
		{
			// Look
			Vector2 lookDirection = controls.Player.Look.ReadValue<Vector2>();
			transform.Rotate(Vector3.up, lookDirection.x * xSensitivity * Time.deltaTime);

			// Movement
			Vector2 movementDirection = controls.Player.Move.ReadValue<Vector2>();
			transform.position = transform.position + new Vector3(movementDirection.x, 0, movementDirection.y) * walkSpeed * Time.deltaTime;
		}
	}
}
