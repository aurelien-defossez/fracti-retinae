using UnityEngine;

// Inspired by https://www.youtube.com/watch?v=tu-Qe66AvtY
[ExecuteInEditMode]
public class CameraShake : MonoBehaviour
{
	[Range(0, 1)]
	[Tooltip("The more trauma, the more the camera shakes")]
	[SerializeField] private float trauma;

	[Tooltip("The rate at which the trauma decreases")]
	[SerializeField] private float traumaDecreaseRate;

	[Tooltip("The maximal offset in each X and Y directions when shaking at trauma=1")]
	[SerializeField] private float translationalForce;

	[Tooltip("The maximal angle in Z rotation when shaking at trauma=1")]
	[SerializeField] private float rotationalForce;

	[Tooltip("The shake frequency")]
	[SerializeField] private float frequency;

	/// <summary>
	/// Do not decrease the trauma over time while PauseDecrease is true
	/// </summary>
	public bool PauseDecrease { get; set; }

	private float minTrauma = 0;
	public float MinTrauma
	{
		get => minTrauma;
		set => minTrauma = Mathf.Clamp01(value);
	}

	protected void Update()
	{
		// Shake value should not be linear to the trauma
		float shake = trauma * trauma;

		// Apply translational force
		transform.localPosition = new Vector2(
			translationalForce * shake * PerlinRange(-1, 1, frequency, .0f),
			translationalForce * shake * PerlinRange(-1, 1, frequency, .5f)
		);

		// Apply rotational force
		transform.localRotation = Quaternion.Euler(0, 0, rotationalForce * shake * PerlinRange(-1, 1, frequency, .25f));

		if (!PauseDecrease)
		{
			// Decrease trauma over time
			trauma = Mathf.Max(0, trauma - traumaDecreaseRate * Time.deltaTime);
		}

		// Ensure a minimal value
		trauma = Mathf.Max(minTrauma, trauma);
	}

	/// <summary>
	/// Add trauma on the camera, which will make it shake
	/// </summary>
	/// <param name="value">The trauma to add</param>
	public void AddTrauma(float value) => trauma = Mathf.Clamp01(trauma + value);

	private float PerlinRange(float min, float max, float frequency, float seed) =>
		min + (max - min) * Mathf.PerlinNoise(frequency * Time.time, seed);
}
