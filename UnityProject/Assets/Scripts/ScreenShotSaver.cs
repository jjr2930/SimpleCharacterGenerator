using SimpleFileBrowser;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class ScreenShotSaver : MonoBehaviour
{
    [SerializeField] GameObject ui;
    Texture2D capturedTexture = null;
    bool dialogOpened = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && !dialogOpened)
        {
            StartCoroutine(CaptureRoutine());
        }
    }

    IEnumerator CaptureRoutine()
    {
        ui.gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();
        capturedTexture = ScreenCapture.CaptureScreenshotAsTexture();

        dialogOpened = true;
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".png"));
        FileBrowser.ShowSaveDialog(onSucess, onCancel, FileBrowser.PickMode.Files);
    }

    private void onCancel()
    {
        dialogOpened = false;
        ui.gameObject.SetActive(true);
    }


    private void onSucess(string[] paths)
    {
        var png = capturedTexture.EncodeToPNG();
        File.WriteAllBytes(paths[0], png);
        dialogOpened = false;
        ui.gameObject.SetActive(true);
    }
}
