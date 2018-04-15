using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : MonoBehaviour {

    public static ThrowProjectile throwControl;

    private void Awake()
    {
        throwControl = this;
    }

    // Use this for initialization

    public void Throw()
    {
        GameObject obj = ObjectPooler.current.GetPooledObjectOne();

        if (obj == null) return;
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.AddComponent<ProjectileMovement>();
        obj.SetActive(true);
    }
}
