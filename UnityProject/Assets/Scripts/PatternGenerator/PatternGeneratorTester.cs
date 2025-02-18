using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PatternGeneratorTester : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] CinemachineTargetGroup targetGroup;

    List<GameObject> patternMeshs = new List<GameObject>();
    public void OnClicked(TextAsset patternFile)
    {
        foreach (var patternMesh in patternMeshs)
        {
            targetGroup.RemoveMember(patternMesh.transform);
            Destroy(patternMesh);
        }

        patternMeshs.Clear();

        var pattern = PatternReader.ReadPatternWithJson(patternFile.text);

        var patternByName = PatternGenerator.GenerateMeshs(pattern);
        foreach (var patternMesh in patternByName)
        {
            GameObject mGo = new GameObject(patternMesh.Key);
            mGo.AddComponent<MeshFilter>().mesh = patternMesh.Value;
            var meshRenderer = mGo.AddComponent<MeshRenderer>();
            meshRenderer.material = Instantiate(material);

            var thisPanel = pattern.pattern.panels[patternMesh.Key];

            Vector3 position = new Vector3{
                x = thisPanel.translation[0],
                y = thisPanel.translation[1],
                z = thisPanel.translation[2]
            };

            Vector3 eulerRotation = new Vector3{
                x = thisPanel.rotation[0],
                y = thisPanel.rotation[1],
                z = thisPanel.rotation[2]
            };

            mGo.transform.position = position;
            mGo.transform.rotation = Quaternion.Euler(eulerRotation);
            mGo.layer = LayerMask.NameToLayer("Pattern");

            targetGroup.AddMember(mGo.transform, 1, 1);

            patternMeshs.Add(mGo);
        }

        // var pointsByName = PatternGenerator.GeneratePoints(pattern);

        // var vertexOriginGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // foreach (var patternMesh in pointsByName)
        // {
        //     GameObject mGo = new GameObject(patternMesh.Key);
        //     var vertices = patternMesh.Value.vertices;
        //     for (int i = 0; i < vertices.Count; i++)
        //     {
        //         var vertexGo = Instantiate(vertexOriginGo);
        //         vertexGo.gameObject.name = "Vertex " + i;
        //         vertexGo.transform.position = vertices[i];
        //         vertexGo.transform.parent = mGo.transform;
        //     }
        // }

        // vertexOriginGo.SetActive(false);
    }
}
