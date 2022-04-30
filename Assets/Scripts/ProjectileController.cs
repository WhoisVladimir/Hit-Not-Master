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
    private float distanceBound = 100f;
    private Vector3 startPoint;

    public int Damage => damage;

    private void FixedUpdate()
    {
        if (isShoot) 
        {
            rb.AddForce(direction * speed, ForceMode.Impulse);
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
        Debug.Log("Shoot.");
        
        rb.useGravity = false;
        this.direction = direction;
        isShoot = true;
        startPoint = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Environment"))
        {
            isShoot = false;
            rb.useGravity = true;
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }
}
