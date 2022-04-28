using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int damage = 1;
    private Vector3 direction;
    private bool isShoot;

    public int Damage => damage;

    private void FixedUpdate()
    {
        if(isShoot) rb.AddForce(direction * speed, ForceMode.Impulse);

    }

    public void Shoot(Vector3 direction)
    {
        this.direction = direction;
        isShoot = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isShoot = false;
        rb.useGravity = true;
    }
}
