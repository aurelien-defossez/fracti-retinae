using UnityEngine;

namespace FractiRetinae
{
    public class MusicManager : MonoBehaviour
    {
        public AudioSource explorationSource;
        public AudioSource glyphHuntSource;

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
