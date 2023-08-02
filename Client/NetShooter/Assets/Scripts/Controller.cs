using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private float _mouseSensetivity = 2f;

    void Update() {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        var space = Input.GetKeyDown(KeyCode.Space);

        _player.SetInputs(h, v, mouseX * _mouseSensetivity);
        _player.RotateX(-mouseY * _mouseSensetivity);
        if (space) _player.Jump();

        SendMove();
    }

    private void SendMove() {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z}
        };

        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
