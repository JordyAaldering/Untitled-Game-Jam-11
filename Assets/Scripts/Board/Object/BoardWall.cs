using UnityEngine;

namespace Board.Object
{
    public class BoardWall : BoardObject
    {
        private readonly MeshData meshData;

        private readonly GameObject gameObject;
        private readonly MeshFilter meshFilter;
        private readonly MeshRenderer meshRenderer;

        public BoardWall(GameObject gameObject)
        {
            this.gameObject = gameObject;
            meshData = new MeshData("Board wall mesh");
            meshFilter = gameObject.GetComponent<MeshFilter>() ?? gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.GetComponent<MeshRenderer>() ?? gameObject.AddComponent<MeshRenderer>();
        }

        public void Clear()
        {
            meshData.Clear();
        }

        public void AddFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            meshData.AddFace(a, b, c, d);
        }

        public void CreateMesh(Material material)
        {
            gameObject.layer = 8;
            Mesh mesh = meshData.CreateMesh();
            meshFilter.sharedMesh = mesh;
            meshRenderer.material = material;
        }
    }
}
