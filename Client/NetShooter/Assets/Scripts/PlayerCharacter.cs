using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed = 2f;

    private float _inputH;
    private float _inputV;

    public void SetInputs(float inputH, float inputV) {
        _inputH = inputH;
        _inputV = inputV;
    }

    void FixedUpdate() {
        Move();
    }

    void Move() {
        //var direction = new Vector3(_inputH, 0, _inputV).normalized;
        //transform.position += direction * _speed * Time.deltaTime;

        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * _speed;
        _rigidbody.velocity = velocity;
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity) {
        position = transform.position;
        velocity = _rigidbody.velocity;
    }
}
