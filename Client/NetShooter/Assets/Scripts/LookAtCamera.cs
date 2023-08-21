using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _transform;
    private Transform _camera;

    private void Awake() => _transform = transform;

    private void Start() => _camera = Camera.main.transform;

    private void Update() => _transform.LookAt(_camera);
}
