using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();

    public void Clear()
    {
        vertices.Clear();
        triangles.Clear();
        normals.Clear();
        uvs.Clear();
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh
        {
            name = "Custom Mesh",
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            normals = normals.ToArray(),
            uv = uvs.ToArray()
        };
        
        return mesh;
    }
}
