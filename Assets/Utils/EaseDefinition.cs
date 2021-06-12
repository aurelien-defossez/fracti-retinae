using System;
using UnityEngine;

/// <summary>
/// A description of how a numerical motion is interpolated, featuring a duration and a type of easing.
/// </summary>
[Serializable]
public struct EaseDefinition
{
	/// <summary>
	/// Gets or sets the duration of the interpolation.
	/// </summary>
	public float duration;

	/// <summary>
	/// Gets or sets the type of ease.
	/// </summary>
	public EaseType ease;

	/// <summary>
	/// Gets the easing function correspondng to the ease type.
	/// </summary>
	public Easer EaserFunction
	{
		get
		{
			if (easerFunction == null)
			{
				easerFunction = Ease.FromType(ease);
			}

			return easerFunction;
		}
	}

	private Easer easerFunction;

	/// <summary>
	/// Constructs an ease definition with a duration and standard ease.
	/// </summary>
	/// <param name="duration">Duration of the interpolation.</param>
	/// <param name="easeType">Type of the standard easer function to use.</param>
	public EaseDefinition(float duration = 0, EaseType easeType = EaseType.Linear)
	{
		this.duration = duration;

		ease = easeType;
		easerFunction = Ease.FromType(ease);
	}

	/// <summary>
	/// Linearly interpolates the result of applying the easer function to <paramref name="t"/>.
	/// The values passed to <see cref="EaserFunction"/> are capped to 1.
	/// </summary>
	/// <param name="a">The start value</param>
	/// <param name="b">The end value</param>
	/// <param name="t">The interpolation value between the two floats</param>
	/// <returns>The interpolated float result between the two float values</returns>
	public float Lerp(float a, float b, float t) => Mathf.Lerp(a, b, EaserFunction(duration > 0 ? Mathf.Clamp01(t / duration) : 1));

	/// <summary>
	/// Linearly interpolates the result of applying the easer function to <paramref name="t"/> without clamping the results.
	/// </summary>
	/// <param name="a">The start value</param>
	/// <param name="b">The end value</param>
	/// <param name="t">The interpolation value between the two floats</param>
	/// <returns>The interpolated float result between the two float values</returns>
	public float LerpUnclamped(float a, float b, float t) => Mathf.LerpUnclamped(a, b, EaserFunction(duration > 0 ? t / duration : 1));
}
