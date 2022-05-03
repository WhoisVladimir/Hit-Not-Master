using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private Rigidbody massCenterRb;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int damage = 1;
    private Vector3 direction;
    private bool isShoot;
    private float distanceBound = 100f;
    private Vector3 startPoint;

    public int Damage => damage;

    private void FixedUpdate()
    {
        if (isShoot) 
        {
            massCenterRb.AddForce(direction * speed, ForceMode.Impulse);
            if (gameObject.activeInHierarchy)
            {
                var distance = Vector3.Distance(startPoint, transform.position);
                if (distance > distanceBound) 
                {
                    gameObject.SetActive(false);
                    isShoot = false;
                }                  
            }
        } 
    }

    public void Shoot(Vector3 direction)
    {
        massCenterRb.useGravity = false;
        this.direction = direction;
        isShoot = true;
        startPoint = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Environment"))
        {
            isShoot = false;
            massCenterRb.useGravity = true;
        }
    }

    private void OnDisable()
    {
        massCenterRb.velocity = Vector3.zero;
    }
}
