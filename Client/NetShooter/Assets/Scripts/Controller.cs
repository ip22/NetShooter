using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    [SerializeField] private float _restartDelay = 3f;
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _gun;

    [SerializeField] private float _mouseSensetivity = 2f;

    private MultiplayerManager _multiplayerManager;

    private bool _hold = false;

    private void Start() {
        _multiplayerManager = MultiplayerManager.Instance;
    }

    void Update() {
        if (_hold) return;

        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        var isShoot = Input.GetMouseButton(0);

        var space = Input.GetKeyDown(KeyCode.Space);

        var isSit = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C) ? true : false;



        _player.SetInputs(h, v, mouseX * _mouseSensetivity);
        _player.RotateX(-mouseY * _mouseSensetivity);

        if (space) _player.Jump();

        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        // *** Homework 2nd week ***
        if (isSit) _player.SitDown();
        else _player.StandUp();

        // *** Homework 3rd week ***


        SendMove(isSit);
    }

    private void SendShoot(ref ShootInfo shootInfo) {
        shootInfo.key = _multiplayerManager.GetSessionID();
        var json = JsonUtility.ToJson(shootInfo);

        _multiplayerManager.SendMessage("shoot", json);
    }

    // *** Homework 2nd week ***
    private void SendMove(bool isSit) {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z},
            {"rX", rotateX},
            {"rY", rotateY},
            {"sit", isSit}
        };

        _multiplayerManager.SendMessage("move", data);
    }

    internal void Restart(string jsonRestartInfo) {
        RestartInfo info = JsonUtility.FromJson<RestartInfo>(jsonRestartInfo);
        StartCoroutine(Hold());

        _player.transform.position = new Vector3(info.x, 0, info.z);
        _player.SetInputs(0, 0, 0);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"pX", info.x},
            {"pY", 0},
            {"pZ", info.z},
            {"vX", 0f},
            {"vY", 0f},
            {"vZ", 0f},
            {"rX", 0f},
            {"rY", 0f},
            {"sit", false}
        };

        _multiplayerManager.SendMessage("move", data);
    }

    private IEnumerator Hold() {
        _hold = true;
        yield return new WaitForSecondsRealtime(_restartDelay);
        _hold = false;
    }
}

[System.Serializable]
public struct ShootInfo
{
    public string key;
    public float pX;
    public float pY;
    public float pZ;
    public float dX;
    public float dY;
    public float dZ;
}

[Serializable]
public struct RestartInfo
{
    public float x;
    public float z;
}