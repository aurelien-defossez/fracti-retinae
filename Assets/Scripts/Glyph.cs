using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FractiRetinae
{
	public class Glyph : MonoBehaviour
	{
		[SerializeField] private float glowStartDistance;
		[SerializeField] private bool traceGlyphVisibility;
		[SerializeField] private AudioSource resonanceSound;
		[SerializeField, Range(0, 2)] private float minPitch = 1;
		[SerializeField, Range(0, 2)] private float maxPitch = 1;
		[SerializeField, Range(0, 10)] private float volumeFadeSpeed = 1;
		[SerializeField] private Color weakColor;
		[SerializeField] private Color fullColor;
		[SerializeField] private Material[] decals;
		[SerializeField] private MeshRenderer[] faces;

		public bool IsVisible { get; private set; } = false;
		public float CenterDistance { get; private set; } = float.PositiveInfinity;
		public float NormalDifference { get; private set; } = 0;

		private int cameraId;
		private Camera viewCamera;
		private Material decal;

		protected void OnEnable()
		{
			cameraId = PlayerController.Instance.GetCameraIndexFromLayer(gameObject.layer);
			viewCamera = PlayerController.Instance.Cameras[cameraId - 1];

			decal = new Material(decals[cameraId - 1]);
			decal.color = weakColor;
			foreach (MeshRenderer face in faces)
			{
				face.material = decal;
			}

			resonanceSound.volume = 0;
			resonanceSound.pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.InverseLerp(1, LevelLoader.Instance.CurrentLevel.CameraCount, cameraId));

			UpdateGlyph();
		}

		public void Update()
		{
			IsVisible = false;
			CenterDistance = float.PositiveInfinity;

			Vector3 headToGlyph = transform.position - PlayerController.Instance.HeadPosition;
			Ray ray = new Ray(PlayerController.Instance.HeadPosition, headToGlyph);
			if (Physics.Raycast(ray, out RaycastHit hit, headToGlyph.magnitude, 1 << gameObject.layer))
			{
				if (hit.collider.gameObject == gameObject)
				{
					IsVisible = true;
					CenterDistance = 2 * Mathf.Abs(viewCamera.WorldToViewportPoint(transform.position).x - 0.5f);
					NormalDifference = 180 - Vector3.Angle(hit.normal, PlayerController.Instance.LookDirection);

					if (traceGlyphVisibility)
					{
						Debug.Log($"Glyph on camera {viewCamera.name} at distance {CenterDistance} with a normal angle difference of {NormalDifference}");
					}
				}
				else if (traceGlyphVisibility)
				{
					Debug.Log($"Glyph on camera {viewCamera.name} is obstructed by {hit.collider.name}");
				}
			}

			UpdateGlyph();
		}

		public void UpdateGlyph()
		{
			float relativeDistance = 1 - Mathf.InverseLerp(LevelLoader.Instance.MaximalGlyphDistance, 1, CenterDistance);

			decal.color = Color.Lerp(weakColor, fullColor, relativeDistance);

			resonanceSound.volume = Mathf.Clamp(relativeDistance,
				resonanceSound.volume - volumeFadeSpeed * Time.deltaTime,
				resonanceSound.volume + volumeFadeSpeed * Time.deltaTime
			);
		}
	}
}
