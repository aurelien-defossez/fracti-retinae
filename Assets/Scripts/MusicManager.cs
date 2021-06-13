using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource explorationSource;
    public AudioSource glyphHuntSource;

    public void OnLevelStart()
    {
        explorationSource.volume = 1.0f;
        glyphHuntSource.volume = 0.0f;
    }
    public void OnGoalFound()
    {
        explorationSource.volume = 0.0f;
        glyphHuntSource.volume = 1.0f;
    }
}
