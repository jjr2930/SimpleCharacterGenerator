using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Pool;

public delegate void OnRequestReceivedDelegate(float[] shapes, string texturePath);

[Serializable]
public class Response
{
    public string gender;
    public float[] shapes;
    public string texturePath;
    public string outputDir;
}

public class AppController : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] ModelGenerator modelGenerator;

    HttpListener httpListener = null;

    OnRequestReceivedDelegate onRequestReceived;

    public void Awake()
    {
        httpListener = new HttpListener();

        var port =  ArgumentParser.GetPort() ;
        
        httpListener.Prefixes.Add(string.Format($"http://+:{port}/"));

        StartCoroutine(StartServer());
    }
    public void OnDestroy()
    {
        if (httpListener != null)
        {
            httpListener.Stop();
            httpListener.Close();
            httpListener = null;
        }
    }
    private IEnumerator StartServer()
    {
        if (!httpListener.IsListening)
        {
            httpListener.Start();
            Console.WriteLine("Server started");

            while (httpListener != null)
            {
                var operation = httpListener.GetContextAsync();

                yield return new WaitUntil(() => operation.IsCompleted );

                var context = operation.Result;
                var bodyStream = context.Request.InputStream;
                var encoding = context.Request.ContentEncoding;
                var reader = new StreamReader(bodyStream, encoding);

                string body = reader.ReadToEnd();

                var response = JsonUtility.FromJson<Response>(body);

                modelGenerator.SetGender(response.gender);
                modelGenerator.SetShape(response.shapes);
                modelGenerator.SetTexture(response.texturePath);

                var imgs = ListPool<Texture2D>.Get();
                {
                    cameraController.Render(imgs, modelGenerator.SelectedShape.gameObject);

                    //image save to directory
                    for (int i = 0; i < imgs.Count; i++)
                    {
                        var img = imgs[i];
                        var bytes = img.EncodeToPNG();
                        var path = Path.Combine(response.outputDir, $"{(CameraController.LocationNames)i}.png");

                        if(!Directory.Exists(response.outputDir))
                            Directory.CreateDirectory(response.outputDir);

                        File.WriteAllBytes(path, bytes);
                    }
                }
                ListPool<Texture2D>.Release(imgs);

                context.Response.Close();
            }
        }
    }
}
