using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: SerializeField] public float speed { get; protected set; } = 2f;
    public Vector3 velocity { get; protected set; }

    // *** Homework ***
    protected bool _isSit;

    public void SitDown() {
        transform.localScale = new Vector3(transform.localScale.x, .5f, transform.localScale.z);
    }

    public void StandUp() {
        transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
    }
}
