using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;

    private Vector3 _targetPosition = Vector3.zero;
    private float _velocityMagnitude = 0f;

    private void Start() {
        _targetPosition = transform.position;
    }

    private void Update() {
        if (_velocityMagnitude > .1f) {
            var maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, maxDistance);
        } else {
            transform.position = _targetPosition;
        }

        if (_isSit) SitDown();
        else StandUp();
    }

    public void SetSpeed(float value) => speed = value;
    public void SetMaxHP(int value) {
        maxHealth = value;
        _health.SetMax(value);
        _health.SetCurrent(value);
    }
    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval) {
        _targetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;

        this.velocity = velocity;
    }

    public void ApplyDamage(int damage) {
        _health.ApplyDamage(damage);
    } 

    // *** Homework 2nd week ***
    public void SetRotateX(float value) => _head.localEulerAngles = new Vector3(Mathf.LerpAngle(_head.localEulerAngles.x, value, Time.deltaTime * 15f), 0f, 0f);
    // public void SetRotateX(float value) => _head.localEulerAngles = new Vector3(value, 0f, 0f);

    public void SetRotateY(float value) => transform.localEulerAngles = new Vector3(0f, Mathf.LerpAngle(_head.localEulerAngles.y, value, Time.deltaTime * 15f), 0f);
    //public void SetRotateY(float value) => transform.localEulerAngles = new Vector3(0f, value, 0f);

    // *** Homework 2nd week***
    internal void SetIsSit(bool isSit) {
        _isSit = isSit;
    }
}
