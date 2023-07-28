using UnityEngine;

public class Test : MonoBehaviour
{
    Example _example;

    URLLoader _URLLoad;

    ////private bool _isMoving;

    void Start() {
        _example.StartRun("url", CallbeckAvito, ErrorAvito);

        _URLLoad.StartRun("url", (s) => { Debug.Log(s); }, (s) => { Debug.LogError(s); });
    }

    private void CallbeckAvito(string s) { }
    private void ErrorAvito(string s) { }

    //private void Update() {
    //    Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

    //    _isMoving = (direction.x == 0 && direction.y == 0);

    //    Debug.Log($"_isMoving: {_isMoving}, direction.x: {direction.x}, direction.y: {direction.y}");
    //}
}
