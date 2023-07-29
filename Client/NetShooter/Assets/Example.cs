using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Example : MonoBehaviour
{
    [SerializeField] private string _url;

    Action _action;

    private void Start() {
        // Action
        StartRun(_url, с”цес, Error);

        // Unnamed
        _action = () => { /* метод */ };

        _action = () => {
            /* метод */
            /* ...   */
        };

        StartRun(_url, с”цес, /* безым€нный метод */(s) => { Debug.Log(s); });
    }

    public void StartRun(string url, Action<string> callback, Action<string> error = null) => StartCoroutine(Run(url, callback, error));

    private IEnumerator Run(string url, Action<string> callback, Action<string> error = null) {
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            error?.Invoke(www.error);
            yield break;
        }

        callback?.Invoke(www.downloadHandler.text);

        // ”ничножаем экземпл€р UnityWebRequest
        www.Dispose();
    }

    private void Error(string error) => Debug.LogError(error);
    private void с”цес(string result) => Debug.Log(result);
}
