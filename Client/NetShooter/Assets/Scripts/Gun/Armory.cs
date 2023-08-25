using System.Collections.Generic;
using UnityEngine;

public class Armory : MonoBehaviour
{
    [SerializeField] private PlayerGun _playerGun;
    [SerializeField] private GunUI _ui;

    List<PlayerGun> _guns = new List<PlayerGun>();

    private void Start() {
        FillArmory();
        SetGun(0);
    }

    private void FillArmory() {
        _guns.Add(new PlayerGun(0, 1, 20f, .2f));
        _guns.Add(new PlayerGun(1, 2, 25f, .15f));
        _guns.Add(new PlayerGun(2, 3, 30f, .1f));
        _guns.Add(new PlayerGun(3, 4, 35f, .05f));
    }

    private int SwitchGun(int index, bool next) {
        if (next) {
            index++;
            if (index > _guns.Count - 1) index = 0;
            return index;

        } else {
            if (index == 0) index = _guns.Count;
            index--;
            return index;
        }
    }

    internal void NextGun() {
        int index = _playerGun.gunIndex;
        SetGun(SwitchGun(index, true));
    }

    internal void PrevGun() {
        var index = _playerGun.gunIndex;
        SetGun(SwitchGun(index, false));
    }

    public void SetGun(int index) {
        _playerGun.gunIndex = _guns[index].gunIndex;
        _playerGun.damage = _guns[index].damage;
        _playerGun.bulletSpeed = _guns[index].bulletSpeed;
        _playerGun.shootDelay = _guns[index].shootDelay;

        _ui.UpdateText(_playerGun.GunInfoLog());
    }

    public byte PlayerGunIndex() => _playerGun.gunIndex;
}
