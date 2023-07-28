using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class URLLoader : MonoBehaviour
{
    public void StartRun(string url, Action<string> callback, Action<string> error = null) => StartCoroutine(Run(url, callback, error));

    private IEnumerator Run(string url, Action<string> callback, Action<string> error = null) {
        // using - экземпл€р класса в круглых скобках существует только в фигурных скобках
        using (UnityWebRequest www = UnityWebRequest.Get(url)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) error?.Invoke(www.error);
            else callback?.Invoke(www.downloadHandler.text);
        }
    }
}
