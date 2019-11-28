using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    public class MeshData
    {
        private readonly string name;

        private readonly List<Vector3> vertices = new List<Vector3>();
        private readonly List<int> triangles = new List<int>();
    
        public MeshData(string name) => this.name = name;

        public void Clear()
        {
            vertices.Clear();
            triangles.Clear();
        }

        public void AddFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            int i = vertices.Count;
            AddTriangles(i, i + 1, i + 2, i + 3);
        
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);
        }
        
        private void AddTriangles(int a, int b, int c, int d)
        {
            AddTriangle(b, c, a);
            AddTriangle(b, d, c);
        }

        private void AddTriangle(int a, int b, int c)
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
            };
            
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
