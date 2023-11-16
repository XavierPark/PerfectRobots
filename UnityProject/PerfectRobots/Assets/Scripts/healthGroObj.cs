using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heaAmmoGroObj : MonoBehaviour
{
    [Header("-----  Components  -----\n")]
    [SerializeField] float floatSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float floatSpeedMin;
    [SerializeField] float floatSpeedMax;
    [SerializeField] float rotateSpeedMin;
    [SerializeField] float rotateSpeedMax;

    [Header("-----  Components  -----\n")]
    [SerializeField] int HealthHolding;


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
        float floatOffset = Mathf.Sin(Time.time * floatSpeed) * 0.5f;
        Vector3 newPosition = initialPosition + new Vector3(0, floatOffset, 0);
        transform.position = newPosition;
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            Mathf.Clamp(player.HP += HealthHolding, 0, player.HPOrig);
            player.updatePlayerUI();
            Destroy(gameObject);
        }
    }
}
