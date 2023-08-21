using UnityEngine;

public class PlayerGun : Gun
{
    public PlayerGun(byte index, int damage, float bulletSpeed, float shootDelay) {
        gunIndex = index;
        this.damage = damage;
        this.bulletSpeed = bulletSpeed;
        this.shootDelay = shootDelay;
    }

    [SerializeField] private Transform _bulletPoint;

    [SerializeField] public int damage { set; get; }
    [SerializeField] public float bulletSpeed { set; get; }
    [SerializeField] public float shootDelay { set; get; }

    // [SerializeField] Gun visual...

    private float _lastShootTime;

    public bool TryShoot(out ShootInfo info) {
        info = new ShootInfo();

        if (Time.time - _lastShootTime < shootDelay) return false;

        Vector3 position = _bulletPoint.position;
        Vector3 velocity = _bulletPoint.forward * bulletSpeed;

        _lastShootTime = Time.time;
        Instantiate(_bulletPrefab, position, _bulletPoint.rotation).Init(velocity, damage);

        shoot?.Invoke();

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = velocity.x;
        info.dY = velocity.y;
        info.dZ = velocity.z;

        return true;
    }

    public string GunInfoLog() => 
        $"#: {gunIndex + 1} | dmg: {damage} | spd: {bulletSpeed} | del: {shootDelay}";
}
