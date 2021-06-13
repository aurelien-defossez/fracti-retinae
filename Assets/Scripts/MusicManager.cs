using UnityEngine;

namespace FractiRetinae
{
	public class MusicManager : MonoBehaviour
	{
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
				explorationSource.volume = 1.0f;
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
				glyphHuntSource.volume = 1.0f;
			}
		}

		public void MuteMusic()
		{
			explorationSource.volume = 0.0f;
			glyphHuntSource.volume = 0.0f;
		}
	}
}
