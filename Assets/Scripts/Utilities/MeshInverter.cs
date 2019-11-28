using System;
using UnityEngine;

namespace Utilities
{
    public class MeshInverter : MonoBehaviour
    {
        public void Awake()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh sharedMesh = meshFilter.sharedMesh;
            
            int[] triangles = sharedMesh.triangles;
            Vector3[] normals = sharedMesh.normals;
            
            for (int i=0 ; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }
            
            for (int i = 0; i < triangles.Length / 3; i++)
            {
                int temp = triangles[i * 3 + 1];
                triangles[i * 3 + 1] = triangles[i * 3];
                triangles[i * 3] = temp;
            }
            
            Mesh mesh = Instantiate(meshFilter.sharedMesh);
            mesh.triangles = triangles;
            mesh.normals = normals;
            meshFilter.sharedMesh = mesh;
        }
    }
}
