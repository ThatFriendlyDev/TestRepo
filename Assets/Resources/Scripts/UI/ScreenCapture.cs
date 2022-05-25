using UnityEngine;
using System.IO;

public class ScreenCapture : MonoBehaviour
{
    public KeyCode captureScreenshotKey = KeyCode.K;
    public enum Format { RAW, JPG, PNG, PPM };
    public Format format = Format.PNG;

    [HideInInspector]
    public string folder;

    private string UniqueFilename()
    {
        if (string.IsNullOrEmpty(this.folder))
        {
            this.folder = Application.dataPath;

            if (Application.isEditor)
            {
                string stringPath = folder + "/..";
                this.folder = Path.GetFullPath(stringPath);
            }

            this.folder += "/screenshots";
            Directory.CreateDirectory(this.folder);
        }

        return string.Format("{0}/screen_{1}.{2}", this.folder, Utils.GetRandomString(10), format.ToString().ToLower());
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(this.captureScreenshotKey))
        {
            string filename = this.UniqueFilename();
            UnityEngine.ScreenCapture.CaptureScreenshot(filename);
            Debug.Log("New screenshot has been captured!");
        }
#endif
    }
}