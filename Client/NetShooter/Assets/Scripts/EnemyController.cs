using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

// ������� ���� �����.
// ������������� ����������� �������� �����.
// � ������� ����� ������ �� ���������� ��������� � ���� �����������.
// ����� ����� ���� ��������������� � ������ ����������.

// ���. ������� �������� - DOP



// ������� �� ��������.


// ��� ������� ���� ������, ��� ������ �� �������� (����)? � ����������.


public class EnemyController : MonoBehaviour
{
    // ** DOP
    // �������������� ����
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
        // ����������� �� ����������� ������� �� ������� � ���������� �� ������� ��������� x � y
        _direction = (position - _lastPosition).normalized;

        // �������� ���������� �� ������� �������� �������, ���� ��� �� ����� 0, ������ ����� ���������
        _isMoved = _input.x != 0 && _input.x != 0;

        _newPosition = position;
        // **

        //transform.position = position;
    }

    public void Update() {

        // ** DOP
        // ���� ����� ���������, ������� ��� � ����������������� �������
        // _isMoved - � ������ ��� �����-�� bool �� ������� ��� ��� � ��� �����
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
