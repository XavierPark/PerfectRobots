using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FullyDamagedItems : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    public GameObject myPrefab;

    void Start()
    {

    }

    void Update()
    {

    }

    public void takeDamage(int dmg)
    {

        HP -= dmg;

        if (HP <= 0)
        {
            Destroy(gameObject);
            GameObject newObject = Instantiate(myPrefab, new Vector3(2.5f, -0.025f, 8.5f), Quaternion.Euler(-90, 0, 0));

        }

    }
}
