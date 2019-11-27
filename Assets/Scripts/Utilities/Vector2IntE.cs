using UnityEngine;

namespace Utilities
{
    public static class Vector2IntE
    {
        public static Vector2Int Rotate90Around(this Vector2Int pointToRotate, Vector2Int centerPoint)
        {
            const float angleInRadians = Mathf.PI / 2;
            float cosTheta = Mathf.Cos(angleInRadians);
            float sinTheta = Mathf.Sin(angleInRadians);
            return new Vector2Int
            (
                Mathf.FloorToInt(cosTheta * (pointToRotate.x - centerPoint.x) - sinTheta * (pointToRotate.y - centerPoint.y) + centerPoint.x),
                Mathf.FloorToInt(sinTheta * (pointToRotate.x - centerPoint.x) + cosTheta * (pointToRotate.y - centerPoint.y) + centerPoint.y)
            );
        }
    }
}
