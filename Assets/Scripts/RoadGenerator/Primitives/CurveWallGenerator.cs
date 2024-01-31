using System.Collections.Generic;
using UnityEngine;

namespace MeshGenRoads {

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CurveWallGenerator : MonoBehaviour, IMeshGenerator
{
    public float radius;
    public float height;
    public uint subdivisions;
    [Space]
    public List<Material> materials;
    public bool flipUvs;
    public bool outside;
    [Space]
    public float startHeight;
    public float endHeight;
    public AnimationCurve interpolation = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public Mesh Generate() {
        if (radius < 0f || height < 0f) {
            Debug.LogError("Radius and height cannot be negative");
            return null;
        }

        List<Vector3> verts = new();
        List<int> inds = new();
        List<Vector2> uvs = new();
        Mesh mesh = new();

        Vector3 point = new(radius, 0f, 0f);
        Vector3 vertical = new(0f, startHeight, 0f);
        Vector3 heightOffset = new(0, height, 0);

        float t = 1f / (subdivisions + 1);

        verts.Add(point + vertical);
        verts.Add(point + vertical + heightOffset);

        uvs.Add(new(1, 0));
        uvs.Add(new(1, 1));

        for (int i = 0; i < subdivisions + 1; i++) {
            point = Quaternion.AngleAxis(t * 90f, Vector3.up) * point;

            float time = interpolation.Evaluate(t * (i + 1));
            vertical.y = Mathf.Lerp(startHeight, endHeight, time);

            verts.Add(point + vertical);
            verts.Add(point + vertical + heightOffset);

            uvs.Add(new(1 - t * (i + 1), 0));
            uvs.Add(new(1 - t * (i + 1), 1));

        }

        for (int i = 0; i < subdivisions + 1; i++) {
            if (outside) {
                inds.Add(2 * i);
                inds.Add(2 * i + 2);
                inds.Add(2 * i + 1);

                inds.Add(2 * i + 1);
                inds.Add(2 * i + 2);
                inds.Add(2 * i + 3);
                continue;
            }
            inds.Add(2 * i);
            inds.Add(2 * i + 1);
            inds.Add(2 * i + 2);

            inds.Add(2 * i + 2);
            inds.Add(2 * i + 1);
            inds.Add(2 * i + 3);
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