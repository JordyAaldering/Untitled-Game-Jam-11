using UnityEngine;

public interface BoardObject
{
    void AddFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d);
    void CreateMesh(Material material, bool useCollider);
}
