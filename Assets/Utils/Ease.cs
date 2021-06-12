/// <summary>
/// An easing function, a transformation of a line.
/// </summary>
/// <param name="t">A parameter transformed by the function. This usually is between 0 and 1.</param>
/// <returns>The transformed value. For values of t between 0 and 1, this usually should be between 0 and 1. 
/// 0 usually gives 0 and 1 usually gives 1.</returns>
public delegate float Easer(float t);

/// <summary>
/// A type of standard easing function.
/// </summary>
public enum EaseType
{
	Linear,
	SmoothStart2, SmoothStop2, SmoothStep2,
	SmoothStart3, SmoothStop3, SmoothStep3,
	BackStart, BackStop, BackStep,
	SmoothStartExp, SmoothStopExp, SmoothStepExp,
	SmoothStartSine, SmoothStopSine, SmoothStepSine,
	ElasticStart, ElasticStop, ElasticStep,
	SmoothBellSine
}

/// <summary>
/// Utilities and common presets of easing functions.
/// </summary>
public static class Ease
{
	public static readonly Easer Linear = (t) => t;
	public static readonly Easer SmoothStart2 = (t) => t * t;
	public static readonly Easer SmoothStop2 = (t) => 1 - SmoothStart2(1 - t);
	public static readonly Easer SmoothStep2 = (t) => (t <= 0.5f) ? SmoothStart2(t * 2) / 2 : SmoothStop2(t * 2 - 1) / 2 + 0.5f;
	public static readonly Easer SmoothStart3 = (t) => t * t * t;
	public static readonly Easer SmoothStop3 = (t) => 1 - SmoothStart3(1 - t);
	public static readonly Easer SmoothStep3 = (t) => (t <= 0.5f) ? SmoothStart3(t * 2) / 2 : SmoothStop3(t * 2 - 1) / 2 + 0.5f;
	public static readonly Easer BackStart = (t) => t * t * (2.70158f * t - 1.70158f);
	public static readonly Easer BackStop = (t) => 1 - BackStart(1 - t);
	public static readonly Easer BackStep = (t) => (t <= 0.5f) ? BackStart(t * 2) / 2 : BackStop(t * 2 - 1) / 2 + 0.5f;
	public static readonly Easer SmoothStartExp = (t) => (float)System.Math.Pow(2, 10 * (t - 1));
	public static readonly Easer SmoothStopExp = (t) => 1 - SmoothStartExp(1 - t);
	public static readonly Easer SmoothStepExp = (t) => t < .5f ? SmoothStartExp(t * 2) / 2 : SmoothStopExp(t * 2 - 1) / 2 + 0.5f;
	public static readonly Easer SmoothStartSine = (t) => 1 - SmoothStopSine(1 - t);
	public static readonly Easer SmoothStopSine = (t) => (float)System.Math.Sin(System.Math.PI / 2 * t);
	public static readonly Easer SmoothStepSine = (t) => 0.5f - (float)System.Math.Cos(System.Math.PI * t) / 2f;
	public static readonly Easer ElasticStart = (t) => 1 - ElasticStop(1 - t);
	public static readonly Easer ElasticStop = (t) => (float)(System.Math.Pow(2, -10 * t) * System.Math.Sin((t - 0.075f) * (2 * System.Math.PI) / 0.3f) + 1);
	public static readonly Easer ElasticStep = (t) => (t <= 0.5f) ? ElasticStart(t * 2) / 2 : ElasticStop(t * 2 - 1) / 2 + 0.5f;
	public static readonly Easer SmoothBellSine = (t) => (float)(1 - System.Math.Cos(t * 2 * System.Math.PI)) / 2;

	/// <summary>
	/// Makes a standard easer.
	/// </summary>
	/// <param name="type">The type of standard easer to make.</param>
	/// <returns>An easer among the preset easers contained in this class.</returns>
	public static Easer FromType(EaseType type)
	{
		switch (type)
		{
			case EaseType.Linear: return Linear;
			case EaseType.SmoothStart2: return SmoothStart2;
			case EaseType.SmoothStop2: return SmoothStop2;
			case EaseType.SmoothStep2: return SmoothStep2;
			case EaseType.SmoothStart3: return SmoothStart3;
			case EaseType.SmoothStop3: return SmoothStop3;
			case EaseType.SmoothStep3: return SmoothStep3;
			case EaseType.BackStart: return BackStart;
			case EaseType.BackStop: return BackStop;
			case EaseType.BackStep: return BackStep;
			case EaseType.SmoothStartExp: return SmoothStartExp;
			case EaseType.SmoothStopExp: return SmoothStopExp;
			case EaseType.SmoothStepExp: return SmoothStepExp;
			case EaseType.SmoothStartSine: return SmoothStartSine;
			case EaseType.SmoothStopSine: return SmoothStopSine;
			case EaseType.SmoothStepSine: return SmoothStepSine;
			case EaseType.ElasticStart: return ElasticStart;
			case EaseType.ElasticStop: return ElasticStop;
			case EaseType.ElasticStep: return ElasticStep;
			case EaseType.SmoothBellSine: return SmoothBellSine;
			default: return Linear;
		}
	}
}
