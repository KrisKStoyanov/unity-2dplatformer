using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleProjectile : MonoBehaviour {

    public static GrappleProjectile Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Grapple()
    {
        GameObject obj = ObjectPooler.current.GetPooledObjectTwo();


        if (obj == null) return;
        obj.AddComponent<GrappleMovement>();
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
    }



}
