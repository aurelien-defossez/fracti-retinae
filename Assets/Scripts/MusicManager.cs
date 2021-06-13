using UnityEngine;

namespace FractiRetinae
{
	public class MusicManager : MonoBehaviourSingleton<MusicManager>
	{
		[SerializeField] private AudioSource explorationSource;

		protected override void Awake()
		{
			base.Awake();

			explorationSource.volume = Cheater.Instance.MuteMusic ? 0 : 1;
		}
	}
}
