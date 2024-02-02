using UnityEditor;
using UnityEngine;
using MeshGenRoads;

[CanEditMultipleObjects]
[CustomEditor(typeof(QuarterPipeGenerator))]
public class QuarterPipeGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate")) {
            ((QuarterPipeGenerator)target).Generate();
        }
    }
}