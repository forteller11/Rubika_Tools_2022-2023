using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Class09Exercise04 : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;
    private List<Vector3> verticeCache = new();
    private List<Vector3> verticeBuffer = new();
    private List<Color> vertexColors = new ();
    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        mesh.GetColors(vertexColors);
        mesh.GetVertices(verticeCache);
        mesh.GetVertices(verticeBuffer);
    }

    public void Update()
    {
        var local2World = transform.localToWorldMatrix;
        var world2Local = transform.worldToLocalMatrix;
        var localDown = -transform.up;
        for (int i = 0; i < verticeCache.Count; i++)
        {
            var vertexLocal = verticeCache[i];
            var vertexGlobal = local2World * new float4(vertexLocal, 1);
            float redChannel = 1;
            if (i < vertexColors.Count)
                redChannel = vertexColors[i].r;
            if (Physics.Raycast(new Ray(vertexGlobal, localDown), out var hit))
            {
                vertexGlobal = hit.point;
                vertexGlobal = Vector3.Lerp(vertexGlobal, hit.point, redChannel);
            }

            vertexGlobal.w = 1;
            var newVertLocal = world2Local * vertexGlobal;
            verticeBuffer[i] = newVertLocal;
        }
    
        mesh.SetVertices(verticeBuffer);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.MarkModified();
    }
}
