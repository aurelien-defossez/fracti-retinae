using UnityEngine;

	public static class Vector3Extensions
	{
		/// <summary>
		/// Set the X value of a vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="x">The new X value</param>
		/// <returns>A copy of the vector with a new X value</returns>
		public static Vector3 WithX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);

		/// <summary>
		/// Set the Y value of a vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="y">The new Y value</param>
		/// <returns>A copy of the vector with a new Y value</returns>
		public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);

		/// <summary>
		/// Set the Z value of a vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="z">The new Z value</param>
		/// <returns>A copy of the vector with a new Z value</returns>
		public static Vector3 WithZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);

		/// <summary>
		/// Increment the X value of a vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="x">The increment value</param>
		/// <returns>A copy of the vector with its X value incremented</returns>
		public static Vector3 WithXIncremented(this Vector3 v, float x) => new Vector3(v.x + x, v.y, v.z);

		/// <summary>
		/// Increment the Y value of a vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="y">The increment value</param>
		/// <returns>A copy of the vector with its Y value incremented</returns>
		public static Vector3 WithYIncremented(this Vector3 v, float y) => new Vector3(v.x, v.y + y, v.z);

		/// <summary>
		/// Increment the Z value of a vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="z">The increment value</param>
		/// <returns>A copy of the vector with its Z value incremented</returns>
		public static Vector3 WithZIncremented(this Vector3 v, float z) => new Vector3(v.x, v.y, v.z + z);

		/// <summary>
		/// Multiply the X value of a vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="x">The factor</param>
		/// <returns>A copy of the vector with its X value multiplied</returns>
		public static Vector3 WithXMultiplied(this Vector3 v, float x) => new Vector3(v.x * x, v.y, v.z);

		/// <summary>
		/// Multiply the Y value of a vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="y">The factor</param>
		/// <returns>A copy of the vector with its Y value multiplied</returns>
		public static Vector3 WithYMultiplied(this Vector3 v, float y) => new Vector3(v.x, v.y * y, v.z);

		/// <summary>
		/// Multiply the Z value of a vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="z">The factor</param>
		/// <returns>A copy of the vector with its Z value multiplied</returns>
		public static Vector3 WithZMultiplied(this Vector3 v, float z) => new Vector3(v.x, v.y, v.z * z);

		/// <summary>
		/// Multiply each component of the vector by the corresponding component of the other vector. 
		/// Resulting vector is { x = v.x*u.x, y = v.y*u.y, z = v.z*u.z }
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="u">The other vector to multiply the components with</param>
		/// <returns>The muliplied vector</returns>
		public static Vector3 WithComponentsMultiplied(this Vector3 v, Vector3 u) => new Vector3(v.x * u.x, v.y * u.y, v.z * u.z);

		/// <summary>
		/// Return a vector with each component being its absolute value.
		/// </summary>
		/// <param name="v">The vector</param>
		/// <returns>A copy of the vector with each component positive</returns>
		public static Vector3 Abs(this Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

		/// <summary>
		/// Cap the magnitude of the given vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="maxMagnitude">The maximal magnitude</param>
		/// <returns>The vector with the given maximal magnitude</returns>
		public static Vector3 CapMagnitude(this Vector3 v, float maxMagnitude) => v.normalized * Mathf.Min(v.magnitude, maxMagnitude);

		/// <summary>
		/// Clamp the vector inside the boundaries.
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="bounds">The boundaries the vector should be in</param>
		/// <returns>The clamped vector</returns>
		public static Vector3 Clamp(this Vector3 v, Bounds bounds)
		{
			if (v.x < bounds.min.x)
			{
				v = v.WithX(bounds.min.x);
			}
			else if (v.x > bounds.max.x)
			{
				v = v.WithX(bounds.max.x);
			}

			if (v.y < bounds.min.y)
			{
				v = v.WithY(bounds.min.y);
			}
			else if (v.y > bounds.max.y)
			{
				v = v.WithY(bounds.max.y);
			}

			return v;
		}

		/// <summary>
		/// Rotate a vector around the Z axis
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="degrees">The rotation in degrees</param>
		/// <returns>The rotated vector</returns>
		public static Vector3 RotateAroundZ(this Vector3 v, float degrees)
		{
			float radians = degrees * Mathf.Deg2Rad;
			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);

			return new Vector3(cos * v.x - sin * v.y, sin * v.x + cos * v.y, v.z);
		}

		/// <summary>
		/// Get the float array representation of the vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <returns>An array of 3 float values, x, y and z</returns>
		public static float[] ToArray(this Vector3 v) => new float[] { v.x, v.y, v.z };

		/// <summary>
		/// Creates a Vector2 using x and y values of the Vector3
		/// </summary>
		/// <param name="v">The Vector3</param>
		/// <returns>The Vector2 with x and y values set from the Vector3</returns>
		public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.x, v.y);

		/// <summary>
		/// Returns a more precise printable version of the vector
		/// </summary>
		/// <param name="v">The vector</param>
		/// <param name="precision">The number of decimal digits</param>
		/// <returns>A printable version of the vector</returns>
		public static string ToString(this Vector3 v, int precision = 4) => v.ToString("G" + precision);
	}
