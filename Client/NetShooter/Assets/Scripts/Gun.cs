using System;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public byte gunIndex;

    public Action shoot;

    [SerializeField] protected Bullet _bulletPrefab;
}
