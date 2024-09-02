using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraController : MonoBehaviour
{
    [Serializable]
    public struct TransformInfo
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    public enum LocationNames 
    {
        Front,
        Back,
        Left,
        Right,
        LeftHand,
        RightHand,
        FrontHead,
        BackHead,
        Count
    }
    [SerializeField] Camera camera;


    [SerializeField] TransformInfo[] transformInfos = new TransformInfo[(int)LocationNames.Count];
    [SerializeField] string outputPath = null;
    public void Awake()
    {
        Assert.IsNotNull(camera);
        
        //set new rendertexture to camera
        camera.targetTexture = new RenderTexture(1024, 1024, 24);
        camera.targetTexture.name = "RenderTexture";
    }

    public void StartRendering(string path)
    {
        StopAllCoroutines();
        outputPath = path;
        StartCoroutine(Run());
    }

    public IEnumerator Run()
    {
        Assert.IsFalse(string.IsNullOrEmpty(outputPath));
        Assert.IsNotNull(camera);
        Assert.IsNotNull(camera.targetTexture);

        for (LocationNames i = LocationNames.Front; i < LocationNames.Count; i++)
        {
            var info = transformInfos[(int)i];
            camera.transform.SetPositionAndRotation(info.position, info.rotation);
            camera.Render();

            yield return new WaitForEndOfFrame();

            var renderTexture = camera.targetTexture;
            RenderTexture.active = renderTexture;
            var tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            if(!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            var filePath = Path.Combine(outputPath, i.ToString() + ".png");
            File.WriteAllBytes(filePath, tex.EncodeToPNG());
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CameraController))]
    public class CameraControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Take Pictures"))
            {
                var folder = EditorUtility.SaveFolderPanel("set folder", "folder", null);
                if(string.IsNullOrEmpty(folder))
                {
                    Debug.LogError("wrong folder");
                    return;
                }

                var camera = (CameraController)target;

                camera.StartRendering(Path.Combine(folder, UnityEngine.Random.Range(0, 1000000).ToString()));
            }
        }
    }
#endif
}
