using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBlasterGO : MonoBehaviour, Interactable
{
    [Header("-----  Components  -----\n")]
    [SerializeField] float floatSpeed = 1;
    [SerializeField] float rotateSpeed = 30;
    [SerializeField] float floatSpeedMin = 1;
    [SerializeField] float floatSpeedMax = 3;
    [SerializeField] float rotateSpeedMin = 30;
    [SerializeField] float rotateSpeedMax = 50;

    [Header("-----  Weapon Values  -----\n")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int Ammo;
    [SerializeField] int AmmoShot;
    [SerializeField] int AmmoMagMax;
    [SerializeField] int AmmoMax;
    [SerializeField] int reloadTime;



    bool isUsing;
    Vector3 initialPosition;
    SphereCollider sphereCollider;

    public void itemUsed()
    {
        GameManager.Instance.itemUsedGBC(gameObject, sphereCollider);
    }

    public void Start()
    {
        if (transform.parent == null)
        {
            floatSpeed = Random.Range(floatSpeedMin, floatSpeedMax);
            rotateSpeed = Random.Range(rotateSpeedMin, rotateSpeedMax);

            transform.Rotate(-90.0f, 0.0f, 0.0f);
            initialPosition = transform.position;
        }
        else
        {
            Ammo = Inventory.instance.LazerAmmo;
            AmmoMax = Inventory.instance.LazerAmmoMax;
        }
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void Update()
    {
        if (transform.parent == null)
        {
            GameManager.Instance.Gravitate(gameObject, floatSpeed, initialPosition, rotateSpeed);
        }
    }
    public void useItem()
    {
        if (!isUsing && AmmoShot < AmmoMagMax && Ammo != 0)
        {
            StartCoroutine(shoot());
        }
        else if (!isUsing && AmmoShot >= AmmoMagMax && Ammo != 0)
        {
            Debug.Log("reloading");
            StartCoroutine(reload());
        }
    }

    IEnumerator shoot()
    {
        isUsing = true;
        AmmoShot += 1;
        Inventory.instance.LazerAmmo -= 1;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            Quaternion rot = Quaternion.LookRotation(hit.point - transform.position);
            Instantiate(bullet, shootPos.position, rot);
        }
        else
        {
            Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);
            Instantiate(bullet, shootPos.position, rot);
        }
        yield return new WaitForSeconds(shootRate);
        isUsing = false;
    }

    IEnumerator reload()
    {
        yield return new WaitForSeconds(reloadTime);
        if (Inventory.instance.LazerAmmo >= AmmoMax)
        {
            Inventory.instance.LazerAmmo -= AmmoMax;
        }
        AmmoShot = 0;
    }

}
