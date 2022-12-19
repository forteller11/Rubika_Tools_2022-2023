using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Class09Exercise02 : MonoBehaviour
{
    public Vector3 Euler = new Vector3(90,0,0);

    private MeshFilter meshFilter;
    private Mesh mesh;
    private List<Vector3> verticeCache = new ();
    private List<Vector3> verticeBuffer = new ();
    private List<Color> colorCache = new ();
    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        mesh.GetVertices(verticeCache);
        mesh.GetColors(colorCache);
    }

    public void Update()
    {
        mesh.GetVertices(verticeBuffer);
        
        for (int i = 0; i < verticeBuffer.Count; i++)
        {
            var redChannel = colorCache[i].r;
            var rotation = Quaternion.Euler(Euler * redChannel);
            verticeBuffer[i] = rotation * verticeCache[i];
        }
        
        mesh.SetVertices(verticeBuffer);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}