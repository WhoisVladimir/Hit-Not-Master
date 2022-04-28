using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int Health => health;
    [SerializeField] int health = 3;

    private List<Rigidbody> rigidbodies;

    private void Start()
    {
        rigidbodies = new List<Rigidbody>();
        rigidbodies.AddRange(GetComponentsInChildren<Rigidbody>());
        SwitchRbKinematic(true);

    }

    private void SwitchRbKinematic(bool isActive)
    {
        foreach (var item in rigidbodies)
        {
            item.isKinematic = isActive;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == "Damaging")
        {
            health -= collision.gameObject.GetComponent<ProjectileController>().Damage;
        }
        if(health == 0)
        {
            SwitchRbKinematic(false);
            var collider = GetComponent<Collider>();
            collider.enabled = false;
        }
    }

}
