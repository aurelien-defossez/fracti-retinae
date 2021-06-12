/*
The MIT License (MIT)

Copyright (c) 2013 UnityPatterns
Copyright (c) 2017 xCIT (https://www.xcit.org)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public delegate bool Predicate();

public static class Auto
{
	// Adapted from:
	// https://github.com/UnityPatterns/AutoMotion/blob/master/Assets/AutoMotion/Scripts/Auto.cs

	#region Generic coroutines

	/// <summary>
	/// Interpolates an object between from and to, using the ease function and a custom linear interpoler.
	/// </summary>
	/// <typeparam name="T">The type of the object to interpolate.</typeparam>
	/// <param name="from">The object to interpolate from.</param>
	/// <param name="to">The object to interpolate to.</param>
	/// <param name="duration">The interpolation duration, in seconds.</param>
	/// <param name="ease">The ease function.</param>
	/// <param name="action">The action to perform with the interpolated object.</param>
	/// <param name="lerp">The linear interpolation to compute values between from and to. The function may compute
	/// clamp or unclamped values freely.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate<T>(T from, T to, float duration, Easer ease, Action<T> action, Func<T, T, float, T> lerp, bool unscaledTime = false)
	{
		// We start after the last frame so the animation starts right away, and not on the next frame, to be more reactive
		float elapsed = Time.deltaTime;

		// Animate
		while (elapsed < duration)
		{
			// Add time
			elapsed = Mathf.MoveTowards(elapsed, duration, unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);

			// Lerp the animation using the given ease method and call the appropriate action
			action(lerp(from, to, ease(elapsed / duration)));

			// Wait one frame
			yield return 0;
		}

		// End the animation by forcing its end state to 1 (otherwise, will stay at 90+ %)
		action(lerp(from, to, ease(1)));

		// We wait a frame at the end because we started a frame early
		// Also, it makes sure the last state of the interpolation stays at least one frame on screen
		yield return 0;
	}

	/// <summary>
	/// Interpolates a value between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="duration">The interpolation duration, in seconds.</param>
	/// <param name="ease">The ease function.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(float from, float to, float duration, Easer ease, Action<float> action, bool unscaledTime = false) =>
		Interpolate(from, to, duration, ease, action, Mathf.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a value between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="duration">The interpolation duration, in seconds.</param>
	/// <param name="ease">The ease function.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(float from, float to, float duration, EaseType ease, Action<float> action, bool unscaledTime = false) =>
		Interpolate(from, to, duration, Ease.FromType(ease), action, Mathf.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a value between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="ease">The ease definition.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(float from, float to, EaseDefinition ease, Action<float> action, bool unscaledTime = false) =>
		Interpolate(from, to, ease.duration, ease.EaserFunction, action, Mathf.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Vector2 between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="duration">The interpolation duration, in seconds.</param>
	/// <param name="ease">The ease function.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Vector2 from, Vector2 to, float duration, EaseType ease, Action<Vector2> action, bool unscaledTime = false) =>
		Interpolate(from, to, duration, Ease.FromType(ease), action, Vector2.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Vector2 between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="ease">The ease definition.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Vector2 from, Vector2 to, EaseDefinition ease, Action<Vector2> action, bool unscaledTime = false) =>
		Interpolate(from, to, ease.duration, ease.EaserFunction, action, Vector2.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Vector2 between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="duration">The interpolation duration, in seconds.</param>
	/// <param name="ease">The ease function.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Vector2 from, Vector2 to, float duration, Easer ease, Action<Vector2> action, bool unscaledTime = false) =>
		Interpolate(from, to, duration, ease, action, Vector2.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Vector3 between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="duration">The interpolation duration, in seconds.</param>
	/// <param name="ease">The ease function.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Vector3 from, Vector3 to, float duration, Easer ease, Action<Vector3> action, bool unscaledTime = false) =>
		Interpolate(from, to, duration, ease, action, Vector3.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Vector3 between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="duration">The interpolation duration, in seconds.</param>
	/// <param name="ease">The ease function.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Vector3 from, Vector3 to, float duration, EaseType ease, Action<Vector3> action, bool unscaledTime = false) =>
		Interpolate(from, to, duration, Ease.FromType(ease), action, Vector3.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Vector3 between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="ease">The ease definition.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Vector3 from, Vector3 to, EaseDefinition ease, Action<Vector3> action, bool unscaledTime = false) =>
		Interpolate(from, to, ease.duration, ease.EaserFunction, action, Vector3.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Quaternion between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="duration">The interpolation duration, in seconds.</param>
	/// <param name="ease">The ease function.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Quaternion from, Quaternion to, float duration, Easer ease, Action<Quaternion> action, bool unscaledTime = false) =>
		Interpolate(from, to, duration, ease, action, Quaternion.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Quaternion between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="duration">The interpolation duration, in seconds.</param>
	/// <param name="ease">The ease function.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Quaternion from, Quaternion to, float duration, EaseType ease, Action<Quaternion> action, bool unscaledTime = false) =>
		Interpolate(from, to, duration, Ease.FromType(ease), action, Quaternion.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Quaternion between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="ease">The ease definition.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Quaternion from, Quaternion to, EaseDefinition ease, Action<Quaternion> action, bool unscaledTime = false) =>
		Interpolate(from, to, ease.duration, ease.EaserFunction, action, Quaternion.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Color between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="duration">The interpolation duration, in seconds.</param>
	/// <param name="ease">The ease function.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Color from, Color to, float duration, Easer ease, Action<Color> action, bool unscaledTime = false) =>
		Interpolate(from, to, duration, ease, action, Color.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Color between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="duration">The interpolation duration, in seconds.</param>
	/// <param name="ease">The ease function.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Color from, Color to, float duration, EaseType ease, Action<Color> action, bool unscaledTime = false) =>
		Interpolate(from, to, duration, Ease.FromType(ease), action, Color.LerpUnclamped, unscaledTime);

	/// <summary>
	/// Interpolates a Color between from and to, using the ease function.
	/// </summary>
	/// <param name="from">The value to interpolate from.</param>
	/// <param name="to">The value to interpolate to.</param>
	/// <param name="ease">The ease definition.</param>
	/// <param name="action">The action to perform with the interpolated value.</param>
	/// <returns>The IEnumerator for the coroutine.</returns>
	public static IEnumerator Interpolate(Color from, Color to, EaseDefinition ease, Action<Color> action, bool unscaledTime = false) =>
		Interpolate(from, to, ease.duration, ease.EaserFunction, action, Color.LerpUnclamped, unscaledTime);

	#endregion

	#region Transform coroutines

	public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, Easer ease, Space space = Space.Self)
	{
		switch (space)
		{
			case Space.World:
				return Interpolate(transform.position, target, duration, ease, v => transform.position = v);
			case Space.Self:
				return Interpolate(transform.localPosition, target, duration, ease, v => transform.localPosition = v);
			default:
				throw new NotSupportedException();
		}
	}

	public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, Space space = Space.Self) =>
		MoveTo(transform, target, duration, Ease.Linear, space);

	public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, EaseType ease, Space space = Space.Self) =>
		MoveTo(transform, target, duration, Ease.FromType(ease), space);

	public static IEnumerator MoveTo(this Transform transform, Vector3 target, EaseDefinition ease, Space space = Space.Self) =>
		MoveTo(transform, target, ease.duration, ease.EaserFunction, space);

	public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration, Easer ease, Space space = Space.Self)
	{
		switch (space)
		{
			case Space.World:
				return Interpolate(target, transform.position, duration, ease, v => transform.position = v);
			case Space.Self:
				return Interpolate(target, transform.localPosition, duration, ease, v => transform.localPosition = v);
			default:
				throw new NotSupportedException();
		}
	}

	public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration, Space space = Space.Self) =>
		MoveFrom(transform, target, duration, Ease.Linear, space);

	public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration, EaseType ease, Space space = Space.Self) =>
		MoveFrom(transform, target, duration, Ease.FromType(ease), space);

	public static IEnumerator MoveFrom(this Transform transform, Vector3 target, EaseDefinition ease, Space space = Space.Self) =>
		MoveFrom(transform, target, ease.duration, ease.EaserFunction, space);

	public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration, Easer ease) =>
		Interpolate(transform.localScale, target, duration, ease, v => transform.localScale = v);

	public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration) =>
		Interpolate(transform.localScale, target, duration, Ease.Linear, v => transform.localScale = v);

	public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration, EaseType ease) =>
		Interpolate(transform.localScale, target, duration, ease, v => transform.localScale = v);

	public static IEnumerator ScaleTo(this Transform transform, Vector3 target, EaseDefinition ease) =>
		Interpolate(transform.localScale, target, ease, v => transform.localScale = v);

	public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration, Easer ease) =>
		Interpolate(target, transform.localScale, duration, ease, v => transform.localScale = v);

	public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration) =>
		Interpolate(target, transform.localScale, duration, Ease.Linear, v => transform.localScale = v);

	public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration, EaseType ease) =>
		Interpolate(target, transform.localScale, duration, ease, v => transform.localScale = v);

	public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, EaseDefinition ease) =>
		Interpolate(target, transform.localScale, ease, v => transform.localScale = v);

	public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration, Easer ease) =>
		Interpolate(transform.localRotation, target, duration, ease, v => transform.localRotation = v);

	public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration) =>
		Interpolate(transform.localRotation, target, duration, Ease.Linear, v => transform.localRotation = v);

	public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration, EaseType ease) =>
		Interpolate(transform.localRotation, target, duration, ease, v => transform.localRotation = v);

	public static IEnumerator RotateTo(this Transform transform, Quaternion target, EaseDefinition ease) =>
		Interpolate(transform.localRotation, target, ease, v => transform.localRotation = v);

	public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration, Easer ease) =>
		Interpolate(target, transform.localRotation, duration, ease, v => transform.localRotation = v);

	public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration) =>
		Interpolate(target, transform.localRotation, duration, Ease.Linear, v => transform.localRotation = v);

	public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration, EaseType ease) =>
		Interpolate(target, transform.localRotation, duration, ease, v => transform.localRotation = v);

	public static IEnumerator RotateFrom(this Transform transform, Quaternion target, EaseDefinition ease) =>
		Interpolate(target, transform.localRotation, ease, v => transform.localRotation = v);

	public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration, Easer ease)
	{
		float elapsed = 0;
		Vector3 start = transform.localPosition;

		while (elapsed < duration)
		{
			elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
			float t = ease(elapsed / duration);
			transform.localPosition = start * (1 - t) * (1 - t) + control * 2 * (1 - t) * t + target * t * t;
			yield return 0;
		}

		transform.localPosition = target;
	}

	public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration) =>
		CurveTo(transform, control, target, duration, Ease.Linear);

	public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration, EaseType ease) =>
		CurveTo(transform, control, target, duration, Ease.FromType(ease));

	public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, EaseDefinition ease) =>
		CurveTo(transform, control, target, ease.duration, ease.EaserFunction);

	public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration, Easer ease)
	{
		Vector3 target = transform.localPosition;
		transform.localPosition = start;
		return CurveTo(transform, control, target, duration, ease);
	}

	public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration) =>
		CurveFrom(transform, control, start, duration, Ease.Linear);

	public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration, EaseType ease) =>
		CurveFrom(transform, control, start, duration, Ease.FromType(ease));

	public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, EaseDefinition ease) =>
		CurveFrom(transform, control, start, ease.duration, ease.EaserFunction);

	public static IEnumerator Shake(this Transform transform, Vector3 amount, float duration)
	{
		Vector3 start = transform.localPosition;
		Vector3 shake = Vector3.zero;

		while (duration > 0)
		{
			duration -= Time.deltaTime;
			shake.Set(UnityEngine.Random.Range(-amount.x, amount.x), UnityEngine.Random.Range(-amount.y, amount.y), UnityEngine.Random.Range(-amount.z, amount.z));
			transform.localPosition = start + shake;
			yield return 0;
		}

		transform.localPosition = start;
	}

	public static IEnumerator Shake(this Transform transform, float amount, float duration) =>
		Shake(transform, Vector3.one * amount, duration);

	#endregion

	#region Visual coroutines

	public static IEnumerator FadeTo(this SpriteRenderer spriteRenderer, Color target, float duration, Easer ease) =>
		Interpolate(spriteRenderer.color, target, duration, ease, c => spriteRenderer.color = c);

	public static IEnumerator FadeTo(this SpriteRenderer spriteRenderer, Color target, float duration) =>
		Interpolate(spriteRenderer.color, target, duration, EaseType.Linear, c => spriteRenderer.color = c);

	public static IEnumerator FadeTo(this SpriteRenderer spriteRenderer, Color target, float duration, EaseType ease) =>
		Interpolate(spriteRenderer.color, target, duration, ease, c => spriteRenderer.color = c);

	public static IEnumerator FadeTo(this SpriteRenderer spriteRenderer, Color target, EaseDefinition ease) =>
		Interpolate(spriteRenderer.color, target, ease, c => spriteRenderer.color = c);

	public static IEnumerator FadeTo(this TextMeshPro text, float alpha, float duration, Easer ease) =>
		Interpolate(text.alpha, alpha, duration, ease, a => text.alpha = a);

	public static IEnumerator FadeTo(this TextMeshPro text, float alpha, float duration) =>
		Interpolate(text.alpha, alpha, duration, EaseType.Linear, a => text.alpha = a);

	public static IEnumerator FadeTo(this TextMeshPro text, float alpha, float duration, EaseType ease) =>
		Interpolate(text.alpha, alpha, duration, ease, a => text.alpha = a);

	public static IEnumerator FadeTo(this TextMeshPro text, float alpha, EaseDefinition ease) =>
		Interpolate(text.alpha, alpha, ease, a => text.alpha = a);

	#endregion

	#region Waiting coroutines

	[Obsolete("Use UnityEngine.WaitForSeconds instead.")]
	public static IEnumerator Wait(float duration)
	{
		while (duration > 0)
		{
			duration -= Time.deltaTime;
			yield return 0;
		}
	}

	[Obsolete("Use UnityEngine.WaitUntil instead.")]
	public static IEnumerator WaitUntil(Predicate predicate)
	{
		while (!predicate())
			yield return 0;
	}

	[Obsolete("Will not work with nested coroutines. Use Xcit.Unity.WaitAll instead.")]
	public static IEnumerator WaitAll(IEnumerable<IEnumerator> enumerators)
	{
		do
		{
			yield return 0;
		} while (enumerators.Count(e => e.MoveNext()) > 0);
	}

	#endregion

	#region Time-based motion

	public static float Loop(float duration, float from, float to, float offsetPercent)
	{
		float range = to - from;
		float total = (Time.time + duration * offsetPercent) * (Mathf.Abs(range) / duration);

		return (range > 0) ?
			from + Time.time - (range * Mathf.FloorToInt((Time.time / range))) :
			from - (Time.time - (Mathf.Abs(range) * Mathf.FloorToInt((total / Mathf.Abs(range)))));
	}

	public static float Loop(float duration, float from, float to) =>
		Loop(duration, from, to, 0);

	public static Vector3 Loop(float duration, Vector3 from, Vector3 to, float offsetPercent) =>
		Vector3.Lerp(from, to, Loop(duration, 0, 1, offsetPercent));

	public static Vector3 Loop(float duration, Vector3 from, Vector3 to) =>
		Vector3.Lerp(from, to, Loop(duration, 0, 1));

	public static Quaternion Loop(float duration, Quaternion from, Quaternion to, float offsetPercent) =>
		Quaternion.Lerp(from, to, Loop(duration, 0, 1, offsetPercent));

	public static Quaternion Loop(float duration, Quaternion from, Quaternion to) =>
		Quaternion.Lerp(from, to, Loop(duration, 0, 1));

	public static float Wave(float duration, float from, float to, float offsetPercent)
	{
		float range = (to - from) / 2;
		return from + range + Mathf.Sin(((Time.time + duration * offsetPercent) / duration) * (Mathf.PI * 2)) * range;
	}

	public static float Wave(float duration, float from, float to) =>
		Wave(duration, from, to, 0);

	public static Vector3 Wave(float duration, Vector3 from, Vector3 to, float offsetPercent) =>
		Vector3.Lerp(from, to, Wave(duration, 0, 1, offsetPercent));

	public static Vector3 Wave(float duration, Vector3 from, Vector3 to) =>
		Vector3.Lerp(from, to, Wave(duration, 0, 1));

	public static Quaternion Wave(float duration, Quaternion from, Quaternion to, float offsetPercent) =>
		Quaternion.Lerp(from, to, Wave(duration, 0, 1, offsetPercent));

	public static Quaternion Wave(float duration, Quaternion from, Quaternion to) =>
		Quaternion.Lerp(from, to, Wave(duration, 0, 1));

	#endregion
}
