using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using UnityEngine.Assertions.Must;
using UnityEngine.Pool;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraController : MonoBehaviour
{
    public const string BONE_NAME = "neck";

    [Serializable]
    public struct CameraInfo
    {
        public Vector3 position;
        public Quaternion rotation;
        public float orthographicSize;
    }

    public enum LocationNames
    {
        Front,
        Back,
        Left,
        Right,
        FrontHead,
        BackHead,
        Count
    }
    [SerializeField] new Camera camera;


    [SerializeField] CameraInfo[] transformInfos = new CameraInfo[(int)LocationNames.Count];

    [SerializeField, Tooltip("just for debug")] GameObject target = null;
    public bool Done { get; set; }

    public void Awake()
    {
        Assert.IsNotNull(camera);
        
        //set new rendertexture to camera
        camera.targetTexture = new RenderTexture(1024, 1024, 24);
        camera.targetTexture.name = "RenderTexture";
    }

    public void Render( List<Texture2D> result, GameObject target)
    {
        Assert.IsNotNull(camera);
        Assert.IsNotNull(camera.targetTexture);

        for (LocationNames i = LocationNames.Front; i < LocationNames.Count; ++i)
        {
            SetCameraPosition(target, i);

            var renderTexture = camera.targetTexture;
            RenderTexture.active = renderTexture;
            var tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            result.Add(tex);
        }
    }

    private void SetCameraPosition(GameObject target, LocationNames location)
    {
        Vector3 position;
        Quaternion rotation;
        float orthographicSize;

        var info = transformInfos[(int)location];
        if (location == LocationNames.FrontHead || location == LocationNames.BackHead)
        {
            var t = GetHeadReleatedCameraInfo(target, info);
            position = t.position;
            rotation = t.rotation;
            orthographicSize = t.orthographicSize;
        }
        else
        {
            position = info.position;
            rotation = info.rotation;
            orthographicSize = info.orthographicSize;
        }

        camera.orthographicSize = orthographicSize;
        camera.transform.SetPositionAndRotation(position, rotation);
        camera.Render();
    }

    public CameraInfo GetHeadReleatedCameraInfo(GameObject target, CameraInfo info)
    {
        Vector3 bonePosition = FindObject(target, BONE_NAME).transform.position;
        var renderer = target.GetComponentInChildren<SkinnedMeshRenderer>();
        var bounds = renderer.bounds;
        var top = bounds.max.y;
        var headHeight = top - bonePosition.y;
        var headCenterY = bonePosition.y + (headHeight / 2);

        var position = bonePosition;
        position.y = headCenterY;
        position.x = 0f;
        position.z = info.position.z;

        CameraInfo result = new CameraInfo();
        result.position = position;
        result.orthographicSize = headHeight;
        result.rotation = info.rotation;
        return result;
    }

    //find object using name recursively
    public static GameObject FindObject(GameObject go, string name)
    {
        if (go.name == name)
            return go;

        for (int i = 0; i < go.transform.childCount; i++)
        {
            var child = go.transform.GetChild(i).gameObject;
            var result = FindObject(child, name);
            if (result != null)
                return result;
        }

        return null;
    }

    public void SetTransformInfo(LocationNames location, CameraInfo info)
    {
        transformInfos[(int)location] = info;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CameraController))]
    public class CameraControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var controller = (CameraController)target;
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                using (var verticalScope = new EditorGUILayout.VerticalScope("Box"))
                {
                    for (LocationNames i = LocationNames.Front; i < LocationNames.Count; ++i)
                    {
                        using (var verticalScope2 = new EditorGUILayout.VerticalScope("Box"))
                        {
                            var oldColor = GUI.contentColor;
                            GUI.contentColor = Color.yellow;
                            {
                                EditorGUILayout.LabelField(i.ToString());
                            }
                            GUI.contentColor = oldColor;

                            var info = controller.transformInfos[(int)i];
                            info.position = EditorGUILayout.Vector3Field("poss", info.position);
                            info.rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rot", info.rotation.eulerAngles));
                            info.orthographicSize = EditorGUILayout.FloatField("Size", info.orthographicSize);
                            controller.transformInfos[(int)i] = info;

                            GUIContent setContent = new GUIContent("From Camera", "camera value insert to this value");
                            GUIContent goContent = new GUIContent("To Camera", "value insert to camera's value");

                            using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button(setContent))
                                {
                                    var cam = controller.camera;
                                    if (cam == null)
                                    {
                                        Debug.LogError("camera is null");
                                        return;
                                    }

                                    var camInfo = new CameraInfo();

                                    camInfo.position = cam.transform.position;
                                    camInfo.rotation = cam.transform.rotation;
                                    camInfo.orthographicSize = cam.orthographicSize;
                                    controller.transformInfos[(int)i] = camInfo;
                                }

                                if (GUILayout.Button(goContent))
                                {
                                    var cam = controller.camera;
                                    if (cam == null)
                                    {
                                        Debug.LogError("camera is null");
                                        return;
                                    }

                                    controller.SetCameraPosition(controller.target, i);
                                }
                            }
                        }
                    }
                }
                if (GUILayout.Button("Take Pictures"))
                {
                    var folder = EditorUtility.SaveFolderPanel("set folder", "folder", null);
                    if (string.IsNullOrEmpty(folder))
                    {
                        Debug.LogError("wrong folder");
                        return;
                    }

                    var textures = ListPool<Texture2D>.Get();
                    {
                        controller.Render(textures, controller.target);
                        for (LocationNames i = LocationNames.Front; i < LocationNames.Count; ++i)
                        {
                            var tex = textures[(int)i];
                            var bytes = tex.EncodeToPNG();
                            File.WriteAllBytes(Path.Combine(folder, i.ToString() + ".png"), bytes);
                        }
                    }
                    ListPool<Texture2D>.Release(textures);

                    Debug.Log("Done!");
                }

                if(changeScope.changed)
                {
                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
#endif
}
