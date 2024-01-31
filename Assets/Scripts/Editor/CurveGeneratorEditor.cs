using UnityEditor;
using UnityEngine;
using MeshGenRoads;

[CustomEditor(typeof(CurveGenerator))]
public class CurveGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate")) {
            ((CurveGenerator)target).Generate();
        }
    }
}