using System.Linq;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();

				if (instance == null)
				{
					instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
				}
			}

			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (Application.isPlaying)
		{
			// TODO: Check if there are multiple instances in the same scene and throw a warning in this case
			// Also, consider resetting the static instance on scene unload
			// But beware of DontDestroyOnLoad objects
			instance = this as T;
		}
	}
}
