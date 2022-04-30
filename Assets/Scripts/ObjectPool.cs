using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> projectiles;

    private void Awake()
    {
        projectiles = new List<GameObject>();
    }
    
    public GameObject GetProjectile(GameObject prefab, Vector3 instancePos)
    {
        if(projectiles.Count > 0)
        {
            var disabledInstance = projectiles.Find(inst => inst.activeInHierarchy == false);
            if (disabledInstance != null)
            {
                disabledInstance.transform.position = instancePos;
                disabledInstance.transform.rotation = Quaternion.Euler(Vector3.up * -90);
                disabledInstance.SetActive(true);
                return disabledInstance;
            }
        }

        var projInstance = GetInstance(prefab, instancePos);
        projectiles.Add(projInstance);
        return projInstance;
    }

    private GameObject GetInstance(GameObject prefab, Vector3 instancePos)
    {
        return Instantiate(prefab, instancePos, Quaternion.Euler(Vector3.up * -90), transform);
    }
}
