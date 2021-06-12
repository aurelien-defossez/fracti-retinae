using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FractiRetinae
{
	public class Glyph : MonoBehaviour
	{
		private static readonly Vector2 SCREEN_CENTER = new Vector2(0.5f, 0.5f);

		[SerializeField] private float glowStartDistance;
		[SerializeField] private bool traceGlyphVisibility;

		public bool IsVisible { get; private set; } = false;
		public float CenterDistance { get; private set; } = float.PositiveInfinity;

		private Camera viewCamera;
		private Material mat;
		private Color fullColor;

		protected void OnEnable()
		{
			viewCamera = PlayerController.Instance.GetCameraFromLayer(gameObject.layer);
			mat = GetComponentInChildren<MeshRenderer>().material;
			fullColor = mat.color;
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
					CenterDistance = 2 * Vector2.Distance(SCREEN_CENTER, viewCamera.WorldToViewportPoint(transform.position));

					if (traceGlyphVisibility)
					{
						Debug.Log($"Glyph on camera {viewCamera.name} at distance {CenterDistance}");
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
			mat.color = Color.Lerp(fullColor, Color.white, Mathf.InverseLerp(LevelLoader.Instance.MaximalGlyphDistance, 1, CenterDistance));
		}
	}
}
