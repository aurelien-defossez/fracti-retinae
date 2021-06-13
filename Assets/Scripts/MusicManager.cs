using UnityEngine;

namespace FractiRetinae
{
	public class MusicManager : MonoBehaviour
	{
		[SerializeField, Range(0, 1)] private float volume;
		[SerializeField] private AudioSource explorationSource;
		[SerializeField] private AudioSource glyphHuntSource;

		public void OnLevelStart()
		{
			if (Cheater.Instance.MuteMusic)
			{
				MuteMusic();
			}
			else
			{
				explorationSource.volume = volume;
				glyphHuntSource.volume = 0.0f;
			}
		}

		public void OnGoalFound()
		{
			if (Cheater.Instance.MuteMusic)
			{
				MuteMusic();
			}
			else
			{
				explorationSource.volume = 0.0f;
				glyphHuntSource.volume = volume;
			}
		}

		public void MuteMusic()
		{
			explorationSource.volume = 0.0f;
			glyphHuntSource.volume = 0.0f;
		}
	}
}
