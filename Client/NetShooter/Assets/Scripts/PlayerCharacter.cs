using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private float _jumpDelay = .2f;

    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;

    [SerializeField] private float _maxHeadAngle = 90;
    [SerializeField] private float _minHeadAngle = -90;

    private float _inputH;
    private float _inputV;
    private float _rotateY;
    private float _currentRotateX;

    private float _jumpTime;

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
        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * speed;
        velocity.y = _rigidbody.velocity.y;
        base.velocity = velocity;

        _rigidbody.velocity = base.velocity;
    }

    private void RotateY() {
        _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
        _rotateY = 0;
    }

    public void RotateX(float value) {
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0f, 0f);
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY) {
        position = transform.position;
        velocity = _rigidbody.velocity;
        rotateX = _head.localEulerAngles.x;
        rotateY = transform.eulerAngles.y;
    }

    public void Jump() {
        if (_checkFly.IsFly) return;
        if (Time.time - _jumpTime < _jumpDelay) return;
        _jumpTime = Time.time;
        _rigidbody.AddForce(0f, _jumpForce, 0f, ForceMode.VelocityChange);
    }
}
