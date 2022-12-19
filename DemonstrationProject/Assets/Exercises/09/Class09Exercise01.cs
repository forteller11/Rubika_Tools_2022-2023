using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Class09Exercise01 : MonoBehaviour
{
    public float MaxJiggle = 0.01f;

    private MeshFilter meshFilter;
    private Mesh mesh;
    private List<Vector3> verticeBuffer = new List<Vector3>();
    
    private void Start()
    {
        //fetch and store all the components
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
    }

    public void Update()
    {
        //this fills up the vertice buffer...
        //we do this so we don't have to create and allocate a new array everyframe
        //which would lead to garbage collection spikes
        mesh.GetVertices(verticeBuffer);
        for (int i = 0; i < verticeBuffer.Count; i++)
        {
            Vector3 jiggleOffset = Random.insideUnitSphere * MaxJiggle * Time.deltaTime;
            verticeBuffer[i] += jiggleOffset;
        }
        
        mesh.SetVertices(verticeBuffer);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.MarkModified();
    }
}
