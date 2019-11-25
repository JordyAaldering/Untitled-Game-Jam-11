using UnityEngine;

public class BoardComponent : BoardObject
{
    private readonly string name;

    private readonly MeshData meshData;
    private GameObject gameObject;
    private Vector3 offset;

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

    public void CreateMesh(Material material, bool useCollider)
    {
        Mesh mesh = meshData.CreateMesh();
        gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;
        
        if (useCollider)
            gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
    }
}
