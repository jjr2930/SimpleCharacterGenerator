using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public static class QuadraticBezierCurve 
{
    public static List<Vector3> GetCurves(Vector3 p0, Vector3 p1, Vector3 p2, int segments, bool includeEndPoint)
    {
        List<Vector3> points = new List<Vector3>(segments);
        int startIndex = includeEndPoint ? 0 : 1;
        int endIndex = includeEndPoint ? segments : segments - 1;

        for (int i = startIndex; i <= endIndex; i++)
        {
            float t = i / (float)segments;
            points.Add(CalculateQuadraticBezierPoint(t, p0, p1, p2));
        }
        return points;
    }

    static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}
