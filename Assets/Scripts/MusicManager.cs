using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace FractiRetinae
{
	public class MusicManager : MonoBehaviourSingleton<MusicManager>
	{
		private const string LOW_PASS = "MusicLowPass";
		private const string RESONANCE = "MusicResonance";
		private const string GLYPHS = "GlyphsVolume";

		[SerializeField, Range(10, 22000)] private float explorationLowPass = 22000;
		[SerializeField, Range(10, 22000)] private float glyphLowPass = 2000;
		[SerializeField, Range(1, 10)] private float explorationResonance = 1;
		[SerializeField, Range(1, 10)] private float glyphResonance = 2;
		[SerializeField] private EaseDefinition fadeEase;
		[SerializeField] private EaseDefinition glyphFadeEase;
		[SerializeField] private AudioMixer mixer;
		[SerializeField] private AudioSource explorationSource;

		private Coroutine fadeRoutine;

		protected override void Awake()
		{
			base.Awake();

			explorationSource.volume = Cheater.Instance.MuteMusic ? 0 : 1;
		}

		public void OnLevelStart() => this.RestartCoroutine(ref fadeRoutine, FadeCore(explorationLowPass, explorationResonance, fadeEase));

		public void OnGoalFound() => this.RestartCoroutine(ref fadeRoutine, FadeCore(glyphLowPass, glyphResonance, fadeEase));

		private IEnumerator FadeCore(float lowPass, float resonance, EaseDefinition ease)
		{
			mixer.GetFloat(LOW_PASS, out float startLowPass);
			mixer.GetFloat(RESONANCE, out float startResonance);

			yield return Auto.Interpolate(0, 1, ease, t =>
			{
				mixer.SetFloat(LOW_PASS, Mathf.Lerp(startLowPass, lowPass, t));
				mixer.SetFloat(RESONANCE, Mathf.Lerp(startResonance, resonance, t));
			});
		}

		public IEnumerator FadeGlyphsOut()
		{
			mixer.GetFloat(GLYPHS, out float initialVolume);
			yield return Auto.Interpolate(initialVolume, -80, glyphFadeEase, v => mixer.SetFloat(GLYPHS, v));
			mixer.SetFloat(GLYPHS, initialVolume);
		}
	}
}
