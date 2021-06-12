using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FractiRetinae
{
	public class PlayerController : MonoBehaviourSingleton<PlayerController>
	{
		[SerializeField, Min(0)] private float walkSpeed = 1;
		[SerializeField, Min(0)] private float xSensitivity = 1;
		[SerializeField, Min(0)] private float ySensitivity = 1;
		[SerializeField, Min(0)] private float interactDistance = 1;
		[SerializeField] private Transform cameras;

		public PlayerControls Controls { get; private set; }
		public CharacterController CharacterController { get; private set; }
		public Rigidbody Rigidbody { get; private set; }

		private Camera firstCamera;

		protected override void Awake()
		{
			CharacterController = GetComponent<CharacterController>();
			Rigidbody = GetComponent<Rigidbody>();
			firstCamera = cameras.GetComponentInChildren<Camera>();
			Controls = new PlayerControls();

			Controls.Player.Interact.performed += OnInteract;
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
			CharacterController.Move(transform.localRotation * new Vector3(movementDirection.x, Physics.gravity.y, movementDirection.y)
				* walkSpeed
				* Time.deltaTime
			);
		}

		public void TeleportPlayer(Vector3 position, Quaternion rotation)
		{
			CharacterController.enabled = false;
			transform.position = position;
			transform.rotation = rotation;
			CharacterController.enabled = true;
		}

		private void OnInteract(InputAction.CallbackContext obj)
		{
			Debug.Log($"Raycast from {cameras.transform.position.ToString(4)} toward {firstCamera.transform.rotation * Vector3.forward}");
			Ray ray = new Ray(cameras.transform.position, firstCamera.transform.rotation * Vector3.forward);
			if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
			{
				Debug.Log($"Raycast hit {hit.collider.gameObject.name}");
				Switch interactableSwitch = hit.collider.GetComponent<Switch>();

				if (interactableSwitch != null)
				{
					interactableSwitch.Activate();
				}
			}
		}
	}
}
