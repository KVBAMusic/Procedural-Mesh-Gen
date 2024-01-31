using UnityEditor;
using UnityEngine;
using MeshGenRoads;

[CustomEditor(typeof(StraightRoadGenerator))]
public class StraightRoadGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate")) {
            ((StraightRoadGenerator)target).Generate();
        }
    }
}