using UnityEngine;
using System;

namespace MeshGenRoads {

[Serializable]
public class SurfaceModifier {
    [SerializeField] private float nearStart = 0;
    [SerializeField] private float farStart = 0;
    [SerializeField] private float nearEnd = 0;
    [SerializeField] private float farEnd = 0;

    [SerializeField] private Easing startEndInterpolation = Easing.EaseInOut;
    [SerializeField] private Easing nearFarInterpolation = Easing.Linear;

    public Easing StartEndEasing => startEndInterpolation;
    public Easing NearFarEasing => nearFarInterpolation;

    public float Evaluate(float startEndT, float nearFarT) {
        float near = Mathf.Lerp(nearStart, nearEnd, EasingFunction.Evaluate(startEndT, startEndInterpolation));
        float far = Mathf.Lerp(farStart, farEnd, EasingFunction.Evaluate(startEndT, startEndInterpolation));
        return Mathf.Lerp(near, far, EasingFunction.Evaluate(nearFarT, nearFarInterpolation));
    }

}

}

