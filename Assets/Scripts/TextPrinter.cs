using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FractiRetinae
{
	public class TextPrinter : MonoBehaviourSingleton<TextPrinter>
	{
		[SerializeField] private Text textbox;
		[SerializeField] private EaseDefinition fadeIn, fadeOut;

		private Coroutine fadeRoutine;

		protected override void Awake()
		{
			base.Awake();

			textbox.color = textbox.color.WithAlpha(0);
		}

		public void PrintText(string text)
		{
			textbox.text = text;
			this.RestartCoroutine(ref fadeRoutine, FadeTo(1, fadeIn));
		}

		public void HideText()
		{
			this.RestartCoroutine(ref fadeRoutine, FadeTo(0, fadeOut));
		}

		private IEnumerator FadeTo(float target, EaseDefinition ease)
		{
			yield return Auto.Interpolate(textbox.color.a, target, ease, a => textbox.color = textbox.color.WithAlpha(a));
		}
	}
}
