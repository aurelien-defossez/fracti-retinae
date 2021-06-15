using UnityEngine;

public static class Vector2Extensions
{
	/// <summary>
	/// Set the X value of a vector
	/// </summary>
	/// <param name="v">The vector</param>
	/// <param name="x">The new X value</param>
	/// <returns>A copy of the vector with a new X value</returns>
	public static Vector2 WithX(this Vector2 v, float x) => new Vector2(x, v.y);

	/// <summary>
	/// Set the Y value of a vector
	/// </summary>
	/// <param name="v">The vector</param>
	/// <param name="y">The new Y value</param>
	/// <returns>A copy of the vector with a new Y value</returns>
	public static Vector2 WithY(this Vector2 v, float y) => new Vector2(v.x, y);

	/// <summary>
	/// Increment the X value of a vector
	/// </summary>
	/// <param name="v">The vector</param>
	/// <param name="x">The increment value</param>
	/// <returns>A copy of the vector with its X value incremented</returns>
	public static Vector2 WithXIncremented(this Vector2 v, float x) => new Vector2(v.x + x, v.y);

	/// <summary>
	/// Increment the Y value of a vector
	/// </summary>
	/// <param name="v">The vector</param>
	/// <param name="y">The increment value</param>
	/// <returns>A copy of the vector with its Y value incremented</returns>
	public static Vector2 WithYIncremented(this Vector2 v, float y) => new Vector2(v.x, v.y + y);

	/// <summary>
	/// Multiply the X value of a vector
	/// </summary>
	/// <param name="v">The vector</param>
	/// <param name="x">The factor</param>
	/// <returns>A copy of the vector with its X value multiplied</returns>
	public static Vector2 WithXMultiplied(this Vector2 v, float x) => new Vector3(v.x * x, v.y);

	/// <summary>
	/// Multiply the Y value of a vector
	/// </summary>
	/// <param name="v">The vector</param>
	/// <param name="y">The factor</param>
	/// <returns>A copy of the vector with its Y value multiplied</returns>
	public static Vector2 WithYMultiplied(this Vector2 v, float y) => new Vector3(v.x, v.y * y);

	/// <summary>
	/// Multiply each components of the vector by the corresponding component of the other vector
	/// Resulting vector is { x = v.x*u.x, y = v.y*u.y }
	/// </summary>
	/// <param name="v">The vector</param>
	/// <param name="u">The other vector to multiply the components with</param>
	/// <returns>The multiplied vector</returns>
	public static Vector2 WithComponentsMultiplied(this Vector2 v, Vector2 u) => new Vector2(v.x * u.x, v.y * u.y);

	/// <summary>
	/// Return a vector with each component being its absolute value.
	/// </summary>
	/// <param name="v">The vector</param>
	/// <returns>A copy of the vector with each component positive</returns>
	public static Vector2 Abs(this Vector2 v) => new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));

	/// <summary>
	/// Cap the magnitude of the given vector
	/// </summary>
	/// <param name="v">The vector</param>
	/// <param name="maxMagnitude">The maximal magnitude</param>
	/// <returns>The vector with the given maximal magnitude</returns>
	public static Vector2 CapMagnitude(this Vector2 v, float maxMagnitude) => v.normalized * Mathf.Min(v.magnitude, maxMagnitude);

	/// <summary>
	/// Rotate a vector around the Z axis
	/// </summary>
	/// <param name="v">The vector</param>
	/// <param name="degrees">The rotation in degrees</param>
	/// <returns>The rotated vector</returns>
	public static Vector2 Rotate(this Vector2 v, float degrees)
	{
		float radians = degrees * Mathf.Deg2Rad;
		float sin = Mathf.Sin(radians);
		float cos = Mathf.Cos(radians);

		return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
	}

	/// <summary>
	/// Check whether the two vectors create an acute angle, i.e. they are roughly pointing toward the similar direction.
	/// </summary>
	/// <param name="v"></param>
	/// <param name="other">The other vector to check</param>
	/// <returns>True if the two vectors create an acute angle (less than 90°).</returns>
	public static bool MakesAcuteAngle(this Vector2 v, Vector2 other) => Vector2.Dot(v, other) > 0;

	/// <summary>
	/// Get the float array representation of the vector
	/// </summary>
	/// <param name="v">The vector</param>
	/// <returns>An array of 2 float values, x and y</returns>
	public static float[] ToArray(this Vector2 v) => new float[] { v.x, v.y };

	/// <summary>
	/// Creates a Vector3 using x and y values of the Vector2, and an optional z value (default to 0)
	/// </summary>
	/// <param name="v">The Vector2</param>
	/// <param name="z">The Z value</param>
	/// <returns>The Vector3 with x and y values set from the Vector2, and Z from the parameter</returns>
	public static Vector3 ToVector3(this Vector2 v, float z = 0) => new Vector3(v.x, v.y, z);

	/// <summary>
	/// Returns a more precise printable version of the vector
	/// </summary>
	/// <param name="v">The vector</param>
	/// <param name="precision">The number of decimal digits</param>
	/// <returns>A printable version of the vector</returns>
	public static string ToString(this Vector2 v, int precision = 4) => v.ToString("G" + precision);
}
