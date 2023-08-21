using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;

    private Vector3 _targetPosition = Vector3.zero;
    private float _velocityMagnitude = 0f;

    private string _sessionID;

    public void Init(string sessionID) {
        _sessionID = sessionID;
    }

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

    public void RestoreHP(int newValue) {
        _health.SetCurrent(newValue);
    }

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval) {
        _targetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;

        this.velocity = velocity;
    }

    public void ApplyDamage(int damage) {
        _health.ApplyDamage(damage);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"id", _sessionID },
            { "value", damage }
        };

        MultiplayerManager.Instance.SendMessage("damage", data);
    }

    public void SetRotateX(float value) =>
        _head.localEulerAngles = new Vector3(Mathf.LerpAngle(_head.localEulerAngles.x, value, Time.deltaTime * 30f), 0f, 0f);


    public void SetRotateY(float value) =>
        transform.localEulerAngles = new Vector3(0f, Mathf.LerpAngle(transform.localEulerAngles.y, value, Time.deltaTime * 30f), 0f);

    internal void SetIsSit(bool isSit) {
        _isSit = isSit;
    }
}
