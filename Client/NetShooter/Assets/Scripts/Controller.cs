using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;

    void Update() {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        _player.SetInputs(h, v);

        SendMove();
    }

    private void SendMove() {
        _player.GetMoveInfo(out Vector3 position);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"x", position.x},
            {"y", position.z}
        };

        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
