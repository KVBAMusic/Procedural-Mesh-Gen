using UnityEngine;
using System.Collections.Generic;

namespace MeshGenRoads {

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class QuarterPipeGenerator : MonoBehaviour, IMeshGenerator
    {
        const float length = 10;
        [Header("Geometry")]
        public float margin = 1;
        public uint subdivisions = 15;
        public uint widthSubdivisions = 15;
        public QuarterpipeMode mode = QuarterpipeMode.Straight;
        [Header("Materials")]
        public List<Material> materials;
        public bool flipUvs;
        private bool flipNormals = false;
        [Header("Modifier")]
        public SurfaceModifier modifier = new();

        public Mesh Generate() {
            if (margin < 0f) {
                Debug.LogError("Margin and cannot be negative");
                return null;
            }

            List<Vector3> verts = new();
            List<int> inds = new();
            List<Vector2> uvs = new();
            Mesh mesh = new();

            Vector3 circlePoint = new(mode == QuarterpipeMode.Inside ? length : 0, length, 0);
            Vector3 circleDir = mode == QuarterpipeMode.Inside ? Vector3.back : Vector3.down;
            Vector3 circleAxis = mode == QuarterpipeMode.Inside ? Vector3.left : Vector3.forward;
            Vector3 currPoint;

            float t = 1f / (subdivisions + 1);
            float wt = 1f / (widthSubdivisions + 1);

            float radius = length - margin;

            float circleAngle = wt * 90f;
            float axisAngle = t * 90f;

            int i;

            for (i = 0; i <= widthSubdivisions + 1; i++) {
                currPoint = circlePoint + circleDir * radius;
                circleDir = Quaternion.Euler(circleAxis * circleAngle) * circleDir;
                verts.Add(currPoint);
                uvs.Add(new(1 - i * wt, flipUvs ? 1 : 0));
            }

            for (i = 0; i < subdivisions + 1; i++) {
                if (mode == QuarterpipeMode.Straight) {
                    circlePoint -= Vector3.forward * t * length;
                }
                if (mode == QuarterpipeMode.Outside) {
                    circleAxis = Quaternion.Euler(0, axisAngle, 0) * circleAxis;
                }
                circleDir = Vector3.down;
                if (mode == QuarterpipeMode.Inside) {
                    circlePoint.x = Mathf.Sin((i + 1) * axisAngle * Mathf.Deg2Rad) * -length + length;
                    circlePoint.z = Mathf.Cos((i + 1) * axisAngle * Mathf.Deg2Rad) * length - length;
                    circleAxis = Quaternion.Euler(0, -axisAngle, 0) * circleAxis;
                    circleDir = (new Vector3(length, length, - length) - circlePoint).normalized;
                }

                for (int j = 0; j <= widthSubdivisions + 1; j++) {
                    currPoint = circlePoint + circleDir * radius;
                    verts.Add(currPoint);
                    uvs.Add(new(1 - j * wt, flipUvs ? 1 - (i + 1) * t : (i + 1) * t ));
                    circleDir = Quaternion.Euler(circleAxis * circleAngle) * circleDir;
                }
            }

            for (int y = 0; y < subdivisions + 1; y++) {
                for (int x = 0; x < widthSubdivisions + 1; x++) {

                    if (flipNormals) {
                        inds.Add(y * ((int)widthSubdivisions + 2) + x);
                        inds.Add(y * ((int)widthSubdivisions + 2) + x + (int)widthSubdivisions + 2);
                        inds.Add(y * ((int)widthSubdivisions + 2) + x + 1);

                        inds.Add(y * ((int)widthSubdivisions + 2) + x + (int)widthSubdivisions + 3);
                        inds.Add(y * ((int)widthSubdivisions + 2) + x + 1);
                        inds.Add(y * ((int)widthSubdivisions + 2) + x + (int)widthSubdivisions + 2);
                        continue;
                    }
                    inds.Add(y * ((int)widthSubdivisions + 2) + x);
                    inds.Add(y * ((int)widthSubdivisions + 2) + x + 1);
                    inds.Add(y * ((int)widthSubdivisions + 2) + x + (int)widthSubdivisions + 2);

                    inds.Add(y * ((int)widthSubdivisions + 2) + x + (int)widthSubdivisions + 3);
                    inds.Add(y * ((int)widthSubdivisions + 2) + x + (int)widthSubdivisions + 2);
                    inds.Add(y * ((int)widthSubdivisions + 2) + x + 1);
                }
            }

            mesh.SetVertices(verts);
            mesh.SetIndices(inds, MeshTopology.Triangles, 0);
            mesh.RecalculateNormals();

            mesh.SetUVs(0, uvs);
            mesh.RecalculateTangents();

            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshRenderer>().SetMaterials(materials);

            return mesh;
        }
    }

}