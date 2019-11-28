using UnityEngine;

namespace Board.Object
{
    public class BoardComponent : BoardObject
    {
        private readonly string name;

        private readonly MeshData meshData;
        private GameObject gameObject;
        private readonly Vector3 offset;

        public BoardComponent(string name, Vector3 offset)
        {
            this.name = name;
            meshData = new MeshData(name + " mesh");
            this.offset = offset;
        }

        public void CreateObject(Transform parent)
        {
            gameObject = new GameObject(name);
            gameObject.transform.parent = parent;
            gameObject.transform.position = offset;
        }

        public void AddFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            meshData.AddFace(a - offset, b - offset, c - offset, d - offset);
        }

        public void CreateMesh(Material material)
        {
            Mesh mesh = meshData.CreateMesh();
            gameObject.layer = 8;
            gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
            
            gameObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(material) 
                { color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f) };

            gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
        }
    }
}
