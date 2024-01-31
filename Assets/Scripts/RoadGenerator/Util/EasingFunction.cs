using UnityEngine;

public static class EasingFunction {
    public static float EaseIn(float t) {
        return 1 - Mathf.Cos(t * Mathf.PI / 2);
    }

    public static float EaseOut(float t) {
        return Mathf.Sin(t * Mathf.PI / 2);
    }

    public static float EaseInOut(float t) {
        return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
    }

    public static float QuadIn(float t) {
        return t * t;
    }

    public static float QuadOut(float t) {
        return 1 + (1 - t) * (t - 1);
    }

    public static float QuadInOut(float t) {
        return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
    }

    public static float CircularIn(float t) {
        return 1 - Mathf.Sqrt(1 - t * t);
    }

    public static float CircularOut(float t) {
        return Mathf.Sqrt(1 - (t - 1) * (t - 1));
    }

    public static float CircularInOut(float t) {
        return t < 0.5f
            ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2
            : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2;
    }

    public static float Evaluate(float t, Easing e) => e switch {
            Easing.Linear => t,
            Easing.EaseIn => EaseIn(t),
            Easing.EaseOut => EaseOut(t),
            Easing.EaseInOut => EaseInOut(t),
            Easing.QuadIn => QuadIn(t),
            Easing.QuadOut => QuadOut(t),
            Easing.QuadInOut => QuadInOut(t),
            Easing.CircularIn => CircularIn(t),
            Easing.CircularOut => CircularOut(t),
            Easing.CircularInOut => CircularInOut(t),
            _ => t
        };
}