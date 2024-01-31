using UnityEditor;
using UnityEngine;
using MeshGenRoads;

[CustomEditor(typeof(CurveWallGenerator))]
public class CurveWallGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate")) {
            ((CurveWallGenerator)target).Generate();
        }
    }
}