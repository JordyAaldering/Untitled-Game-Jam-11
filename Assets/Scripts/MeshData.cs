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

    public void AddFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        int vertexStart = vertices.Count;
        
        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);
        vertices.Add(d);
        
        AddTriangles(vertexStart, vertexStart + 1, vertexStart + 2, vertexStart + 3);
    }
    
    public void AddTriangles(int a, int b, int c, int d)
    {
        AddTriangle(b, c, a);
        AddTriangle(b, d, c);
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles.Add(a);
        triangles.Add(b);
        triangles.Add(c);
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
