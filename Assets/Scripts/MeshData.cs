using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    private readonly string name;
    
    public readonly List<Vector3> vertices = new List<Vector3>();
    public readonly List<int> triangles = new List<int>();
    public readonly List<Vector3> normals = new List<Vector3>();
    public readonly List<Vector2> uvs = new List<Vector2>();

    public MeshData(string name)
    {
        this.name = name;
    }
    
    public void Clear()
    {
        vertices.Clear();
        triangles.Clear();
        normals.Clear();
        uvs.Clear();
    }

    public void AddQuad(int v00, int v01, int v10, int v11)
    {
        triangles.Add(v00);
        triangles.Add(v01);
        triangles.Add(v10);
        
        triangles.Add(v01);
        triangles.Add(v11);
        triangles.Add(v10);
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh
        {
            name = name,
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray()
        };

        if (normals.Count > 0) mesh.normals = normals.ToArray();
        else mesh.RecalculateNormals();

        return mesh;
    }
}
