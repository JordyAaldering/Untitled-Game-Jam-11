using UnityEngine;

namespace Board.Object
{
    public interface BoardObject
    {
        void AddFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d);
    }
}
