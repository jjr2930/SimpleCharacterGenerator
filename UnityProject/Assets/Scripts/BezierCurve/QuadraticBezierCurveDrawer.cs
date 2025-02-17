using UnityEngine;

//very simple script to draw a quadratic bezier curve
//every point is sphere with radius 0.1
//in inspector can set bezier curve's segment count
//using QuadraticBezierCurve.cs

public class QuadraticBezierCurveDrawer : MonoBehaviour
{
    public Transform point0;
    public Transform point1;
    public Transform point2;
    public float sphereScale = 0.1f;
    public int segmentCount = 20;

    void OnDrawGizmos()
    {
        if (point0 == null || point1 == null || point2 == null)
            return;

        var points = QuadraticBezierCurve.GetCurves(point0.position, point1.position, point2.position, segmentCount, true);
        
        for(int i = 0; i< points.Count; i++)
        {
            Gizmos.DrawSphere(points[i], sphereScale);
            if (i > 0)
            {
                Gizmos.DrawLine(points[i - 1], points[i]);
            }
        }
    }
}
