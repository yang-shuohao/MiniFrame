
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LogUploader
{
    public static void UploadLog(string url, string logFilePath)
    {
        MonoMgr.Instance.StartCoroutine(UploadLogCoroutine(url, logFilePath));
    }

    private static IEnumerator UploadLogCoroutine(string url, string logFilePath)
    {
        if (!File.Exists(logFilePath))
        {
            LogMgr.Instance.LogError("Log file not found.");
            yield break;
        }

        byte[] fileData = File.ReadAllBytes(logFilePath);

        // Create a form and add the file data
        WWWForm form = new WWWForm();
        form.AddBinaryData("logFile", fileData, Path.GetFileName(logFilePath), "text/plain");

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                LogMgr.Instance.LogError($"Log upload failed: {www.error}");
            }
            else
            {
                LogMgr.Instance.Log("Log upload successful.");
            }
        }
    }
}
