using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CurveGenerator))]
public class CurveGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate")) {
            ((CurveGenerator)target).Generate();
        }
    }
}