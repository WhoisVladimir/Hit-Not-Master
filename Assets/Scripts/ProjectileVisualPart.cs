using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVisualPart : MonoBehaviour
{
    [SerializeField] private int rotationSpeed = 10;
    private GameObject parentGO;
    private Rigidbody parentRb;

    private void Start()
    {
        parentGO = transform.parent.gameObject;
        parentRb = parentGO.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!parentRb.useGravity)
        {
            transform.Rotate(Vector3.right * rotationSpeed, Space.Self);
        }
    }

    private void OnBecameInvisible()
    {
        parentGO.SetActive(false);
    }

    private void OnDisable()
    {
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}
