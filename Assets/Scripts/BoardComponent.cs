using UnityEngine;

public class BoardComponent
{
    private string name;
    public readonly MeshData meshData;
    public GameObject gameObject;

    public BoardComponent(string name)
    {
        this.name = name;
        meshData = new MeshData(name);
    }

    public void CreateObject(Transform parent)
    {
        gameObject = new GameObject(name);
        gameObject.transform.parent = parent;
    }

    public void BuildMesh(Material material)
    {
        Mesh mesh = meshData.CreateMesh();
        gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;
    }
}