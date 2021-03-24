using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static Vector3 NormalizedWithBounds(Vector3 point, Vector3 A, Vector3 B){
            var minX = Mathf.Min(A.x, B.x);
            var minY = Mathf.Min(A.y, B.y);
            var maxX = Mathf.Max(A.x, B.x);
            var maxY = Mathf.Max(A.y, B.y);

            return new Vector3(Mathf.Clamp(point.x, minX, maxX), Mathf.Clamp(point.y, minY, maxY));
        }
    
        public static float Vector3InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            var AB = b - a;
            var AV = value - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }
    }
}