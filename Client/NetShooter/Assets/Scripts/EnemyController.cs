using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _enemyCharacter;
    private List<float> _recievedTimeIntervals = new List<float>() { 0f, 0f, 0f, 0f, 0f };
    private float AvarageInterval {
        get {
            int recievedTimeIntervalsCount = _recievedTimeIntervals.Count;
            var sum = 0f;

            for (int i = 0; i < recievedTimeIntervalsCount; i++) {
                sum += _recievedTimeIntervals[i];
            }
            return sum / recievedTimeIntervalsCount;
        }
    }

    private float _lastRecievedTime = 0f;

    private void SaveRecievedTime() {
        var interval = Time.time - _lastRecievedTime;
        _lastRecievedTime = Time.time;

        _recievedTimeIntervals.Add(interval);
        _recievedTimeIntervals.Remove(0);
    }

    internal void OnChange(List<DataChange> changes) {
        SaveRecievedTime();

        var position = _enemyCharacter.TargetPosition;
        var velocity = Vector3.zero;

        foreach (var dataChange in changes) {
            switch (dataChange.Field) {
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    velocity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;
                default:
                    break;
            }
        }

        _enemyCharacter.SetMovement(position, velocity, AvarageInterval);
    }
}
