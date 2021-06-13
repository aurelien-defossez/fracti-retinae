using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FractiRetinae
{
	public class SlidingPlatform : MonoBehaviour
	{
		[SerializeField] private Vector3 translation;
		[SerializeField] private Vector3 rotation;
		[SerializeField] private EaseDefinition animationEase;
		[SerializeField] private bool startMoved;
		[SerializeField] private AudioSource switchSound;

		public bool IsMoved { get; private set; }

		private Vector3 initialPosition;
		private Quaternion initialRotation;
		private Coroutine slideRoutine;

		protected void Awake()
		{
			initialPosition = transform.localPosition;
			initialRotation = transform.localRotation;
		}

		protected void Start()
		{
			if (startMoved)
			{
				SlideToTargetPosition();
			}
		}

		public void Switch()
		{
			if (IsMoved)
			{
				SlideToInitialPosition();
			}
			else
			{
				SlideToTargetPosition();
			}

			if (switchSound != null)
			{
				switchSound.Play();
			}
		}

		public void SlideToInitialPosition()
		{
			IsMoved = false;
			this.RestartCoroutine(ref slideRoutine, MoveTo(initialPosition, initialRotation));
		}

		public void SlideToTargetPosition()
		{
			IsMoved = true;
			this.RestartCoroutine(ref slideRoutine, MoveTo(initialPosition + translation, initialRotation * Quaternion.Euler(rotation)));
		}

		private IEnumerator MoveTo(Vector3 toPosition, Quaternion toRotation)
		{
			Vector3 fromPosition = transform.localPosition;
			Quaternion fromRotation = transform.localRotation;

			yield return Auto.Interpolate(0, 1, animationEase, t =>
			{
				transform.localPosition = Vector3.Lerp(fromPosition, toPosition, t);
				transform.localRotation = Quaternion.Lerp(fromRotation, toRotation, t);
			});
		}
	}
}
