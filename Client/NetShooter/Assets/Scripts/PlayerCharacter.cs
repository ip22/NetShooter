using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;

    private float _inputH;
    private float _inputV;

    public void SetInputs(float inputH, float inputV) {
        _inputH = inputH;
        _inputV = inputV;
    }

    void Update() {
        Move();
    }

    void Move() {
        var direction = new Vector3(_inputH, 0, _inputV).normalized;
        transform.position += direction * _speed * Time.deltaTime;
    }

    public void GetMoveInfo(out Vector3 position) {
        position = transform.position;
    }
}
