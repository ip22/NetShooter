using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _enemyCharacter;
    [SerializeField] private EnemyGun _gun;

    // *** Homework 3nd week ***
    [SerializeField] private EnemyGunUI _ui;

    private List<float> _recievedTimeIntervals = new List<float>() { 0f, 0f, 0f, 0f, 0f };
    private float avarageInterval {
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
    private Player _player;

    public void Init(string key, Player player) {
        _enemyCharacter.Init(key);

        this._player = player;
        _enemyCharacter.SetMaxHP(player.maxHP);
        _enemyCharacter.SetSpeed(player.speed);

        // *** Homework 3nd week ***
        _ui.IndicateGun(_gun.gunIndex);

        player.OnChange += OnChange;
    }
    //internal void SetGun(ShootInfo gunInfo) {
    //    throw new NotImplementedException();
    //}

    public void Shoot(in ShootInfo info) {
        Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
        Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);

        _gun.Shoot(position, velocity);
    }

    public void Destroy() {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }

    private void SaveRecievedTime() {
        var interval = Time.time - _lastRecievedTime;
        _lastRecievedTime = Time.time;

        _recievedTimeIntervals.Add(interval);
        _recievedTimeIntervals.Remove(0);
    }

    internal void OnChange(List<DataChange> changes) {
        SaveRecievedTime();

        var position = transform.position;
        var velocity = _enemyCharacter.velocity;

        foreach (var dataChange in changes) {
            switch (dataChange.Field) {
                case "loss":
                    MultiplayerManager.Instance.lossCounter.SetEnemyLoss((byte)dataChange.Value);
                    break;

                case "currentHP":
                    if ((sbyte)dataChange.Value > (sbyte)dataChange.PreviousValue) _enemyCharacter.RestoreHP((sbyte)dataChange.Value);
                    break;

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

                case "rX":
                    _enemyCharacter.SetRotateX((float)dataChange.Value, avarageInterval);
                    break;

                case "rY":
                    _enemyCharacter.SetRotateY((float)dataChange.Value, avarageInterval);
                    break;

                // *** Homework 2nd week ***
                case "sit":
                    _enemyCharacter.SetIsSit((bool)dataChange.Value);
                    break;

                default:
                    Debug.LogWarning("Can't processed changing of field " + dataChange.Field);
                    break;
            }
        }

        _enemyCharacter.SetMovement(position, velocity, avarageInterval);
    }

    // *** Homework 3nd week ***
    internal void SetGun(GunInfo gunInfo) {
        _gun.gunIndex = gunInfo.index;
        _ui.IndicateGun(_gun.gunIndex);
    }
}
