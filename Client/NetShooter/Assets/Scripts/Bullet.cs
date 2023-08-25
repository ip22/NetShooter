using System.Collections;
using UnityEngine;

public enum BulletHit { Body = 1, Head = 4 }

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private Rigidbody _rigidbody;
    private int _damage;

    public void Init(Vector3 velocity, int damage = 0) {
        _damage = damage;
        _rigidbody.velocity = velocity;
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy() {
        yield return new WaitForSeconds(_lifeTime);
        Destroy();
    }

    private void Destroy() {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.TryGetComponent(out EnemyCharacter enemy)) {
            var point = collision.contacts[0];
            if (collision.collider is SphereCollider) {
                enemy.ApplyDamage(_damage, BulletHit.Head, point);
            } else {
                enemy.ApplyDamage(_damage, BulletHit.Body, point);
            }
          ;
        }
        Destroy();
    }
}
