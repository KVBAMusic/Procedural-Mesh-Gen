using UnityEngine;

namespace MeshGenRoads {
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

    public static float ExponentialIn(float t) {
        return t == 0f ? 0f : Mathf.Pow(2, 10 * t - 10);
    }

    public static float ExponentialOut(float t) {
        return t == 1f ? 1f : Mathf.Pow(2, -10f * t);
    }

    public static float ExponentialInOut(float t) {
            if (t == 0) return 0;
            if (t == 1) return 1;
            if (t < .5f) return Mathf.Pow(2, 20 * t - 10) / 2;
            return (2 - Mathf.Pow(2, -20 * t + 10)) / 2;
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
            Easing.ExponentialIn => ExponentialIn(t),
            Easing.ExponentialOut => ExponentialOut(t),
            Easing.ExponentialInOut => ExponentialInOut(t),
            _ => t
        };
}
}
