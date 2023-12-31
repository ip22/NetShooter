using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private Armory _armory;

    [SerializeField] private float _restartDelay = 3f;
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private GameObject[] _rayVisuals;

    [SerializeField] private float _mouseSensetivity = 2f;

    private MultiplayerManager _multiplayerManager;

    private bool _hold = false;
    private bool _hideCursor;

    private void Start() {
        _multiplayerManager = MultiplayerManager.Instance;
        _hideCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _hideCursor = !_hideCursor;
            Cursor.lockState = _hideCursor ? CursorLockMode.Locked : CursorLockMode.None;
            foreach (var rayVisual in _rayVisuals) { rayVisual.SetActive(_hideCursor); }
        }

        if (_hold) return;

        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        var mouseX = 0f;
        var mouseY = 0f;
        var isShoot = false;

        if (_hideCursor) {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            isShoot = Input.GetMouseButton(0);
        }

        var space = Input.GetKeyDown(KeyCode.Space);

        var isSit = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C) ? true : false;

        var nextGun = Input.GetKeyDown(KeyCode.E);
        var prevGun = Input.GetKeyDown(KeyCode.Q);

        _player.SetInputs(h, v, mouseX * _mouseSensetivity);
        _player.RotateX(-mouseY * _mouseSensetivity);

        if (space) _player.Jump();

        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        if (isSit) _player.SitDown();
        else _player.StandUp();

        if (nextGun) {
            _armory.NextGun();
            SendGunChange(_armory.PlayerGunIndex());
        }

        if (prevGun) {
            _armory.PrevGun();
            SendGunChange(_armory.PlayerGunIndex());
        }

        SendMove(isSit);
    }

    private void SendShoot(ref ShootInfo shootInfo) {
        shootInfo.key = _multiplayerManager.GetSessionID();
        var json = JsonUtility.ToJson(shootInfo);

        _multiplayerManager.SendMessage("shoot", json);
    }

    private void SendGunChange(byte index) {
        GunInfo gun = new GunInfo();
        gun.id = _multiplayerManager.GetSessionID();
        gun.index = index;

        var json = JsonUtility.ToJson(gun);
        _multiplayerManager.SendMessage("gun", json);
    }

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

    public void Restart(int spawnIndex) {
        _multiplayerManager.spawnPoints.GetPoint(spawnIndex, out Vector3 position, out Vector3 rotation);

        StartCoroutine(Hold());

        _player.transform.position = position;

        rotation.x = 0f;
        rotation.z = 0f;
        _player.transform.eulerAngles = rotation;

        _player.SetInputs(0, 0, 0);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", 0f},
            {"vY", 0f},
            {"vZ", 0f},
            {"rX", 0f},
            {"rY", rotation.y},
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
public struct GunInfo
{
    public string id;
    public byte index;
}