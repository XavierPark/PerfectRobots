using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoGO : MonoBehaviour
{
    [Header("-----  Components  -----\n")]
    [SerializeField] float floatSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float floatSpeedMin;
    [SerializeField] float floatSpeedMax;
    [SerializeField] float rotateSpeedMin;
    [SerializeField] float rotateSpeedMax;

    [Header("-----  Components  -----\n")]
    [SerializeField] int AmmoHolding;


    Vector3 initialPosition;
    SphereCollider sphereCollider;


    public void itemUsed()
    {
        //GroundObjectController.instance.itemUsedGBC(gameObject, sphereCollider);
    }

    public void Start()
    {
        if (transform.parent == null)
        {
            floatSpeed = Random.Range(floatSpeedMin, floatSpeedMax);
            rotateSpeed = Random.Range(rotateSpeedMin, rotateSpeedMax);
            initialPosition = transform.position;
        }
    }

    public void Update()
    {
        if (transform.parent == null)
        {
            GroundObjectController.instance.Gravitate(gameObject, floatSpeed, initialPosition, rotateSpeed);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        Inventory.instance.CollectAmmo(AmmoHolding);
        Destroy(gameObject);
    }
}
