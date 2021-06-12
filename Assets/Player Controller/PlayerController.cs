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
		[SerializeField] private Transform cameras;

		public PlayerControls Controls { get; private set; }

		private CharacterController characterController;

		protected void Awake()
		{
			Controls = new PlayerControls();
			characterController = GetComponent<CharacterController>();
		}

		protected void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			Controls.Player.Enable();
		}

		protected void Update()
		{
			// Read input
			Vector2 lookDirection = Controls.Player.Look.ReadValue<Vector2>();
			Vector2 movementDirection = Controls.Player.Move.ReadValue<Vector2>();

			// Look Horizontal
			transform.Rotate(Vector3.up, lookDirection.x * xSensitivity * Time.deltaTime);

			// Look Vertical
			float verticalRotation = cameras.eulerAngles.x - lookDirection.y * ySensitivity * Time.deltaTime;
			cameras.eulerAngles = cameras.eulerAngles.WithX(verticalRotation);

			// Movement
			characterController.Move(transform.localRotation * new Vector3(movementDirection.x, Physics.gravity.y, movementDirection.y)
				* xSensitivity
				* Time.deltaTime
			);
		}
	}
}
