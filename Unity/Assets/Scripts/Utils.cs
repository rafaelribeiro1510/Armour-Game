using UnityEngine;

public static class Utils
{
    public static Vector3 NormalizedWithBounds(Vector3 point, Vector3 A, Vector3 B){
        float minX = Mathf.Min(A.x, B.x);
        float minY = Mathf.Min(A.y, B.y);
        float maxX = Mathf.Max(A.x, B.x);
        float maxY = Mathf.Max(A.y, B.y);

        return new Vector3(Mathf.Clamp(point.x, minX, maxX), Mathf.Clamp(point.y, minY, maxY));
    }
    
    public static float Vector3InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
}