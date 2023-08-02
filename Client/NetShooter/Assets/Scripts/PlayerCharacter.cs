using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _speed = 2f;

    [SerializeField] private Transform _head;

    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _maxHeadAngle = 90;
    [SerializeField] private float _minHeadAngle = -90;

    [SerializeField] private float _jumpForce = 5f;

    private float _inputH;
    private float _inputV;
    private float _rotateY;
    private float _currentRotateX;

    private void Start() {
        var camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
    }

    public void SetInputs(float inputH, float inputV, float rotateY) {
        _inputH = inputH;
        _inputV = inputV;
        _rotateY += rotateY;
    }

    void FixedUpdate() {
        Move();
        RotateY();
    }

    void Move() {
        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * _speed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
    }

    private void RotateY() {
        _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
        _rotateY = 0;
    }

    public void RotateX(float value) {
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0f, 0f);
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity) {
        position = transform.position;
        velocity = _rigidbody.velocity;
    }

    private bool _isFly = true;

    private void OnCollisionStay(Collision collision) {
        var contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++) {
            if (contactPoints[i].normal.y >.45f)_isFly = false;
        }
        _isFly = false;
    }

    public void Jump() {
        if (_isFly) return;
        _rigidbody.AddForce(0f, _jumpForce, 0f, ForceMode.VelocityChange);
    }
}
