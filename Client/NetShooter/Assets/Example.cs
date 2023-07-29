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
        StartRun(_url, �����, Error);

        // Unnamed
        _action = () => { /* ����� */ };

        _action = () => {
            /* ����� */
            /* ...   */
        };

        StartRun(_url, �����, /* ���������� ����� */(s) => { Debug.Log(s); });
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

        // ���������� ��������� UnityWebRequest
        www.Dispose();
    }

    private void Error(string error) => Debug.LogError(error);
    private void �����(string result) => Debug.Log(result);
}
