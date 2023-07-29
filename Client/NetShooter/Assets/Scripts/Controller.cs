using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;

    void Update() {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        _player.SetInputs(h, v);

        SendMove(h, v);
    }

    private void SendMove(float h, float v) {
        _player.GetMoveInfo(out Vector3 position);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"x", position.x},
            {"y", position.z},
            
            // ** DOP
            // добавил в словарь данные
            {"h", h},
            {"v", v}
            // **

        };

        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
