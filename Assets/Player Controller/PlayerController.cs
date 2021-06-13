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
		[SerializeField, Min(0)] private float lookSpeedCap = 10;
		[SerializeField, Range(0, 90)] private float lookDownCap = 90;
		[SerializeField, Range(0, 90)] private float lookUpCap = 90;
		[SerializeField, Min(0)] private float interactDistance = 1;
		[SerializeField] private Transform cameraContainer;

		public PlayerControls Controls { get; private set; }
		public CharacterController CharacterController { get; private set; }
		public Rigidbody Rigidbody { get; private set; }
		public Camera[] Cameras { get; private set; }
		public int[] Layers { get; private set; }

		public Vector3 HeadPosition => cameraContainer.transform.position;
		public Vector3 LookDirection => Cameras.First().transform.rotation * Vector3.forward;
		public Ray LookRay => new Ray(HeadPosition, LookDirection);

		protected override void Awake()
		{
			base.Awake();

			CharacterController = GetComponent<CharacterController>();
			Rigidbody = GetComponent<Rigidbody>();
			Cameras = GetComponentsInChildren<Camera>();
			Controls = new PlayerControls();
			Layers = Enumerable.Range(1, Cameras.Length).Select(i => LayerMask.NameToLayer("Camera" + i)).ToArray();

			Controls.Player.Interact.performed += OnInteract;
		}

		protected void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		protected void Update()
		{
			if (Controls.Player.enabled)
			{
				// Read input
				Vector2 lookDirection = Controls.Player.Look.ReadValue<Vector2>().CapMagnitude(lookSpeedCap);
				Vector2 movementDirection = Controls.Player.Move.ReadValue<Vector2>();

				// Look Horizontal
				transform.Rotate(Vector3.up, lookDirection.x * xSensitivity * Time.deltaTime);

				// Look Vertical
				float verticalRotation = cameraContainer.eulerAngles.x - lookDirection.y * ySensitivity * Time.deltaTime;
				verticalRotation = (verticalRotation + 360) % 360;
				verticalRotation = verticalRotation < 180 ? verticalRotation : verticalRotation - 360;
				verticalRotation = Mathf.Clamp(verticalRotation, -lookUpCap, lookDownCap);
				cameraContainer.eulerAngles = cameraContainer.eulerAngles.WithX(verticalRotation);

				// Movement
				CharacterController.Move(transform.localRotation * new Vector3(movementDirection.x, Physics.gravity.y, movementDirection.y)
					* walkSpeed
					* Time.deltaTime
				);
			}
			else
			{
				CharacterController.Move(Vector3.zero);
			}
		}

		public void TeleportPlayer(Vector3 position, Quaternion rotation)
		{
			CharacterController.enabled = false;
			transform.position = position;
			transform.rotation = rotation;
			cameraContainer.localRotation = Quaternion.identity;
			CharacterController.enabled = true;
		}

		public int GetCameraIndexFromLayer(int layer) => Convert.ToInt32(LayerMask.LayerToName(layer).Last().ToString());

		public IEnumerator LookAt(Vector3 target, EaseDefinition ease)
		{
			Vector3 direction = target - HeadPosition;
			Quaternion toRotation = Quaternion.LookRotation(direction);

			yield return Auto.Interpolate(transform.localRotation, toRotation, ease, r => transform.rotation = r);
		}

		private void OnInteract(InputAction.CallbackContext context) => StartCoroutine(InteractCore());

		private IEnumerator InteractCore()
		{
			// Synchronize on the update loop for reasons
			yield return 0;
			CastInteractRay();
		}

		private void CastInteractRay()
		{
			Debug.Log($"Raycast from {HeadPosition.ToString(4)} toward {LookDirection}");
			RaycastHit[] hits = Physics.RaycastAll(LookRay, interactDistance);
			int defaultLayer = LayerMask.NameToLayer("Default");
			bool[] hitDetected = Enumerable.Repeat(false, LevelLoader.Instance.CurrentLevel.CameraCount).ToArray();

			foreach (RaycastHit hit in hits)
			{
				// Hit something in all views
				if (hit.collider.gameObject.layer == defaultLayer)
				{
					Debug.Log($"Raycast hit {hit.collider.gameObject.name} on all cameras");

					// Stop
					return;
				}
				else
				{
					for (int i = 0; i < hitDetected.Length; i++)
					{
						// New hit on view #i
						if (!hitDetected[i] && hit.collider.gameObject.layer == Layers[i])
						{
							Debug.Log($"Raycast hit {hit.collider.gameObject.name} on camera {i + 1}");

							// Prevent next hits
							hitDetected[i] = true;

							// Activate switch if any
							Switch interactableSwitch = hit.collider.GetComponent<Switch>();
							if (interactableSwitch != null)
							{
								interactableSwitch.Activate();

								// Authorize only one activation per click
								return;
							}
						}
					}
				}
			}
		}
	}
}
