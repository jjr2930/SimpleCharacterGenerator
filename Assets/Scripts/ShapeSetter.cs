using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ShapeSetter : MonoBehaviour
{
    [SerializeField] MinMaxFloat bodyShapeMinMax = new MinMaxFloat(-500 , 500);
    [SerializeField] MinMaxFloat expressionMinMax = new MinMaxFloat(-200 , 200);
    [SerializeField] new SkinnedMeshRenderer renderer = null;

    public void Awake()
    {
        var blendShapeCount = renderer.sharedMesh.blendShapeCount;
        //print blendshpae parameter key name
        for (int i = 0; i < blendShapeCount; i++)
        {
            print(renderer.sharedMesh.GetBlendShapeName(i));
        }
    }

    /// <summary>
    /// 0~10 -> shape 000 ~ shape 009, 11~10 -> Exp000 ~ Exp009
    /// </summary>
    /// <param name="values"></param>
    public void SetShape(float[] values)
    {
        if (values.Length != 20)
            throw new InvalidOperationException($"value length({values.Length}) was not invalid");

        var blendShapeCount = renderer.sharedMesh.blendShapeCount;
        for (int i = 0; i < values.Length; i++)
        {
            var value = values[i];
            renderer.SetBlendShapeWeight(i, value);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ShapeSetter))]
    public class ShapeSetterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("random"))
            {
                float[] randomValues = new float[20];
                for (int i = 0; i < 10; i++)
                {
                    randomValues[i] = UnityEngine.Random.Range(-500, 500);
                }

                for (int i = 10; i < 20; i++)
                {
                    randomValues[i] = UnityEngine.Random.Range(-200, 200);
                }
                ((ShapeSetter)target).SetShape(randomValues);
            }
        }
    }
#endif
}
