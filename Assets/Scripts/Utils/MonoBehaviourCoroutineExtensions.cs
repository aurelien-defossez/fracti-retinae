using System.Collections;
using UnityEngine;

public static class MonoBehaviourCoroutineExtensions
{
	/// <summary>
	/// Starts the coroutine if the game object is active.
	/// </summary>
	/// <param name="m"></param>
	/// <param name="routineMethod">The routine to start.</param>
	/// <returns>The started coroutine, null if the game object was inactive.</returns>
	public static Coroutine TryStartCoroutine(this MonoBehaviour m, IEnumerator routineMethod, System.Action fallback = null)
	{
		if (m.gameObject.activeInHierarchy)
		{
			// Stop the current Coroutine
			return m.StartCoroutine(routineMethod);
		}
		else
		{
			fallback?.Invoke();
			return null;
		}
	}

	/// <summary>
	/// Stops the coroutine if it not null.
	/// </summary>
	/// <param name="m"></param>
	/// <param name="currentRoutine">The coroutine to stop</param>
	public static void TryStopCoroutine(this MonoBehaviour m, ref IEnumerator currentRoutine)
	{
		// Stop the current Coroutine
		if (currentRoutine != null)
		{
			m.StopCoroutine(currentRoutine);
			currentRoutine = null;
		}
	}

	/// <summary>
	/// Stops the coroutine if it not null.
	/// </summary>
	/// <param name="m"></param>
	/// <param name="currentRoutine">The coroutine to stop</param>
	public static void TryStopCoroutine(this MonoBehaviour m, ref Coroutine currentRoutine)
	{
		// Stop the current Coroutine
		if (currentRoutine != null)
		{
			m.StopCoroutine(currentRoutine);
			currentRoutine = null;
		}
	}

	/// <summary>
	/// Start the given coroutine, without expecting a return value, to prevent async tasks to create warnings
	/// if we explecitely don't want to wait for the coroutine.
	/// </summary>
	/// <param name="m"></param>
	/// <param name="routineMethod">The routine to start.</param>
	/// <remarks>The execution of a coroutine can be paused at any point using the yield statement. The yield return value specifies when the coroutine is resumed. Coroutines are excellent when modelling behaviour over several frames. Coroutines have virtually no performance overhead. StartCoroutine function always returns immediately, however you can yield the result. This will wait until the coroutine has finished execution. There is no guarantee that coroutines end in the same order that they were started, even if they finish in the same frame.</remarks>
	public static void StartCoroutineAndForget(this MonoBehaviour m, IEnumerator routineMethod, bool onlyIfActive = false)
	{
		if (!onlyIfActive || m.gameObject.activeInHierarchy)
		{
			m.StartCoroutine(routineMethod);
		}
	}

	/// <summary>
	/// Stops the current coroutine (if any), then starts the new coroutine.
	/// </summary>
	/// <param name="m"></param>
	/// <param name="currentRoutine">The Coroutine object to stop (if not null). This object will be updated with the new Coroutine object.</param>
	/// <param name="routineMethod">The routine to start.</param>
	/// <param name="onlyIfActive">If true, will not try to start the coroutine if game object is inactive.</param>
	/// <returns></returns>
	/// <remarks>The execution of a coroutine can be paused at any point using the yield statement. The yield return value specifies when the coroutine is resumed. Coroutines are excellent when modelling behaviour over several frames. Coroutines have virtually no performance overhead. StartCoroutine function always returns immediately, however you can yield the result. This will wait until the coroutine has finished execution. There is no guarantee that coroutines end in the same order that they were started, even if they finish in the same frame.</remarks>
	public static Coroutine RestartCoroutine(this MonoBehaviour m, ref Coroutine currentRoutine, IEnumerator routineMethod, bool onlyIfActive = false)
	{
		if (!onlyIfActive || m.gameObject.activeInHierarchy)
		{
			// Stop the current Coroutine
			m.TryStopCoroutine(ref currentRoutine);

			// Start and store the new Coroutine
			currentRoutine = m.StartCoroutine(routineMethod);

			// Return routine (consistency with MonoBehaviour.StartCoroutine method)
			return currentRoutine;
		}
		else
		{
			return null;
		}
	}
}
