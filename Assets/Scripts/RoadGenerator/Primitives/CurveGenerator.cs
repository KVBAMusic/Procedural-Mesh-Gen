using System.Collections.Generic;
using UnityEngine;

namespace MeshGenRoads {

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CurveGenerator : MonoBehaviour, IMeshGenerator {
    [Header("Geometry")]
    public float margin = 1;
    public CornerSize cornerSize;
    public uint subdivisions;
    public uint widthSubdivisions;
    [Header("Materials")]
    public List<Material> materials;
    public bool flipUvs;
    [Header("Modifier")]
    public bool absolute = false;
    public SurfaceModifier modifier = new();

        public float Radius => cornerSize switch {
            CornerSize.Small => 10f,
            CornerSize.Medium => 20f,
            CornerSize.Large => 30f,
            _ => 10f
        };

    public Mesh Generate() {
        if (margin < 0f) {
            Debug.LogError("Margin cannot be negative");
            return null;
        }

        float nearRadius = margin + Radius - 10f;
        float farRadius = Radius - margin;

        List<Vector3> verts = new();
        List<int> inds = new();
        List<Vector2> uvs = new();
        Mesh mesh = new();

        Vector3 nearPoint = new(nearRadius, 0f, 0f);
        Vector3 farPoint = new(farRadius, 0f, 0f);
        Vector3 currPoint;
        Vector3 currVertical = Vector3.up;

        float t = 1f / (subdivisions + 1);
        float wt = 1f / (widthSubdivisions + 1);

        int i;

        for (i = 0; i <= widthSubdivisions + 1; i++) {
            currPoint = Vector3.Lerp(nearPoint, farPoint, i * wt);
            if (absolute) {
                currVertical.y = modifier.Evaluate(-currPoint.z / Radius, currPoint.x / Radius);
            }
            else {
                currVertical.y = modifier.Evaluate(0, i * wt);
            }

            verts.Add(currPoint + currVertical);
            uvs.Add(new(1 - i * wt, flipUvs ? 1 : 0));
        }

        for (i = 0; i < subdivisions + 1; i++) {
            nearPoint = Quaternion.AngleAxis(t * 90f, Vector3.up) * nearPoint;
            farPoint = Quaternion.AngleAxis(t * 90f, Vector3.up) * farPoint;

            for (int j = 0; j <= widthSubdivisions + 1; j++) {
                currPoint = Vector3.Lerp(nearPoint, farPoint, j * wt);
                if (absolute) {
                    currVertical.y = modifier.Evaluate(-currPoint.z / Radius, currPoint.x / Radius);
                }
                else {
                    currVertical.y = modifier.Evaluate((i + 1) * t, j * wt);
                }

                verts.Add(currPoint + currVertical);
                uvs.Add(new(1 - j * wt, flipUvs ? 1 - (i + 1) * t : (i + 1) * t ));
            }

        }

        for (int y = 0; y < subdivisions + 1; y++) {
            for (int x = 0; x < widthSubdivisions + 1; x++) {
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

