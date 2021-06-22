using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FractiRetinae
{
	public class Switch : MonoBehaviour
	{
		[SerializeField] private SlidingPlatform[] platforms;
		[SerializeField, Min(0)] private float activationRotation;
		[SerializeField] private Transform lever;
		[SerializeField] private MeshRenderer leverMesh;
		[SerializeField] private EaseDefinition animationEase;
		[SerializeField] private AudioSource switchSound;
		[SerializeField] private Color[] emissionColors;

		private bool isLeft;
		private Coroutine switchRoutine;

		protected void Start()
		{
			leverMesh.material.SetColor("_EmissionColor", emissionColors[PlayerController.Instance.GetCameraIndexFromLayer(gameObject.layer) - 1]);
		}

		protected void OnEnable()
		{
			isLeft = true;
			lever.transform.localRotation = Quaternion.Euler(0, 0, -activationRotation);
		}

		public void Activate()
		{
			Debug.Log($"Activate {name}");

			this.RestartCoroutine(ref switchRoutine, Rotate());

			foreach (SlidingPlatform platform in platforms)
			{
				platform.Switch();
			}

			if (switchSound != null)
			{
				switchSound.Play();
			}
		}

		private IEnumerator Rotate()
		{
			float to = isLeft ? activationRotation : -activationRotation;
			isLeft = !isLeft;

			yield return lever.transform.RotateTo(Quaternion.Euler(0, 0, to), animationEase);
		}
	}
}
