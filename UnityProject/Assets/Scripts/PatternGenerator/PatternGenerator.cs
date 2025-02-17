using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public static class PatternGenerator
{
    public class Curve
    {
        public int[] endpoints {get;set;} = new int[2]{0,0};
        public List<Vector3> points {get;set;} = new List<Vector3>(32);
    }
    
    public class Vertices
    {
        public List<Vector3> vertices = new List<Vector3>(256);
    }

    public static Dictionary<string, Mesh> GenerateMeshs(PatternDataStructures pattern, int bezierCurveSegments = 32)
    {
        Dictionary<string, Vertices> points = GeneratePoints(pattern, bezierCurveSegments);

        Dictionary<string, Mesh> meshes = new Dictionary<string, Mesh>(16);

        foreach (var panel in points)
        {
            Mesh mesh = new Mesh();

            var center = new Vector3(0.0f, 0.0f, 0.0f);
            panel.Value.vertices.ForEach(v => center += v);
            center /= panel.Value.vertices.Count;

            Vector3[] vertices = new Vector3[panel.Value.vertices.Count + 1];
            vertices[0] = center;

            for(int i = 0; i<panel.Value.vertices.Count; i++)
            {
                vertices[i + 1] = panel.Value.vertices[i];
            }
            
            mesh.vertices = vertices;

            List<int> indices = new List<int>(256);
            for (int j = 0; j < panel.Value.vertices.Count; j++)
            {
                indices.Add(0);
                indices.Add(j + 1);
                indices.Add((j + 1) % panel.Value.vertices.Count + 1);
            }

            mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);

            mesh.RecalculateNormals();

            meshes.Add(panel.Key, mesh);
        }

        return meshes;
    }
    public static Dictionary<string, Vertices> GeneratePoints(PatternDataStructures pattern, int bezierCurveSegments = 32)
    {
        Dictionary<string, Vertices> meshes = new Dictionary<string, Vertices>(16);

        foreach (var panel in pattern.pattern.panels)
        {
            Vertices v = new Vertices();
            List<Vector3> vertices = new List<Vector3>(16);
            List<Curve> curves = new List<Curve>(16);

            foreach (var vertex in panel.Value.vertices)
            {
                vertices.Add(new Vector3((float)vertex[0], (float)vertex[1], 0.0f));
            }

            foreach (var edge in panel.Value.edges)
            {
                Vector3 p0 = vertices[edge.endpoints[0]];
                Vector3 p1 = vertices[edge.endpoints[1]];
                if (null != edge.curvature)
                {
                    Curve curve = new Curve();
                    curve.endpoints[0] = edge.endpoints[0];
                    curve.endpoints[1] = edge.endpoints[1];

                    Debug.Log("Curvature: " + edge.curvature[0] + ", " + edge.curvature[1]);
                    float c0 = edge.curvature[0];
                    float c1 = edge.curvature[1];

                    if (pattern.properties.curvature_coords == "relative")
                    {
                        Vector3 localXAxis = (p1 - p0);
                        //외적하는 순서가 중요함
                        Vector3 localYAxis = Vector3.Cross(localXAxis, -Vector3.forward);
                        Vector3 controlPointer = p0 + localXAxis * c0;
                        controlPointer += localYAxis * c1;

                        curve.points.AddRange(QuadraticBezierCurve.GetCurves(p0, controlPointer, p1, bezierCurveSegments,false));
                        curves.Add(curve);
                    }
                }
                else
                {
                    Curve curve = new Curve();
                    curve.endpoints[0] = edge.endpoints[0];
                    curve.endpoints[1] = edge.endpoints[1];

                    curve.points.Add(p0);
                    curve.points.Add(p1);

                    curves.Add(curve);
                }
            }
            
            foreach (var curve in curves)
            {
                for (int i = 0; i < curve.points.Count; i++)
                {
                    v.vertices.Add(curve.points[i]);
                }
            }

            meshes.Add(panel.Key, v);
        }

        return meshes;
    }
}
