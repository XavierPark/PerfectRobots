using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedFullyObjects : MonoBehaviour, IDamage
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

        Debug.Log(gameObject.name);
        HP -= dmg;

        if (HP <= 0)
        {
            Destroy(gameObject);
            GameObject newObject = Instantiate(myPrefab, new Vector3(-3, -2, 11), Quaternion.Euler(-90, 0, 0);
            
        }

    }
}
