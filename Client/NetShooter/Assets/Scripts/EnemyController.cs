using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Windows;

public class EnemyController : MonoBehaviour
{
    // **
    [SerializeField] private EnemyCharacter _enemyCharacter;

    private Vector3 _lastPosition;
    private Vector2 _input;

    private Vector3 _direction;

    private bool _isStoped;
    // **

    internal void OnChange(List<DataChange> changes) {
        Vector3 position = transform.position;

        _lastPosition = position;


        foreach (var dataChange in changes) {
            switch (dataChange.Field) {
                case "x":
                    position.x = (float)dataChange.Value;
                    break;
                case "y":
                    position.z = (float)dataChange.Value;
                    break;
                case "h":
                    _input.x = (float)dataChange.Value;
                    break;
                case "v":
                    _input.y = (float)dataChange.Value;
                    break;
                default:
                    break;
            }
        }
        
        // **
        _direction = (position - _lastPosition).normalized;

        _isStoped = _input.x == 0 && _input.y == 0;
        // **

        //transform.position = position;
    }

    public void Update() {

        // **
        if (_isStoped) AllegedMove();
        // **

    }

    // **
    private void AllegedMove() {
        ////transform.position += _direction * 2 * Time.deltaTime;


        transform.Translate(_direction.normalized * Time.deltaTime * 2, Space.World);

        //var direction = new Vector3(_direction.x, 0, _direction.y).normalized;
        //transform.position += direction * 2 * Time.deltaTime;
    }
    // **
}
