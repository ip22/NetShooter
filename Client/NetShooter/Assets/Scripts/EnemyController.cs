using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

// «адумка была такой.
// ¬ысчитываетс€ направление движени€ енеми.
// ¬ моменты когда лагает он продолжает двигатьс€ в этом направлении.
// ѕотом через лерп синхронизирутс€ с верным положением.

// ƒоп. задание отмечено - DOP



// ¬ообщем не работает.


//  ак клиенту дать пон€ть, что сервер не отвечвет (лаги)? — колизиусом.


public class EnemyController : MonoBehaviour
{
    // ** DOP
    // дополнительные пол€
    private Vector3 _lastPosition;
    private Vector3 _newPosition;
    private Vector2 _input;

    private Vector3 _direction;

    private bool _isMoved;
    // **

    internal void OnChange(List<DataChange> changes) {
        Vector3 position = transform.position;

        _lastPosition = position;

        MultiplayerManager.Instance.
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

        // ** DOP
        // направление от фактической позиции до позиции в полученных от сервера значени€х x и y
        _direction = (position - _lastPosition).normalized;

        // проверка полученных от сервера значений интпута, если они не равны 0, значит енеми двигаетс€
        _isMoved = _input.x != 0 && _input.x != 0;

        _newPosition = position;
        // **

        //transform.position = position;
    }

    public void Update() {

        // ** DOP
        // если енеми двигаетс€, двигать его в предположительную сторону
        // _isMoved - в теории это какой-то bool от сервера что нет с ним св€зи
        if (_isMoved) AllegedMove();
        else SyncPosition();
        // **

    }

    // ** DOP
    private void AllegedMove() {
        transform.Translate(_direction.normalized * Time.deltaTime * 2, Space.World);
    }

    public void SyncPosition() {
        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * 20);
    }
    // **
}
