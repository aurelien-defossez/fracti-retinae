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

		public PlayerControls Controls { get; private set; }

		protected void Awake()
		{
			Controls = new PlayerControls();
		}

		protected void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			Controls.Player.Enable();
		}

		protected void Update()
		{
			// Look
			Vector2 lookDirection = Controls.Player.Look.ReadValue<Vector2>();
			transform.Rotate(Vector3.up, lookDirection.x * xSensitivity * Time.deltaTime);

			// Movement
			Vector2 movementDirection = Controls.Player.Move.ReadValue<Vector2>();
			transform.position = transform.position + transform.localRotation * new Vector3(movementDirection.x, 0, movementDirection.y) * walkSpeed * Time.deltaTime;
		}
	}
}
