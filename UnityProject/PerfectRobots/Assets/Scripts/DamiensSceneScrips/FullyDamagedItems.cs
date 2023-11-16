using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FullyDamagedItems : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    [SerializeField] GameObject intactGlass;
    [SerializeField] GameObject brokenGlass;

    void Start()
    {
        intactGlass.SetActive(true);
        brokenGlass.SetActive(false);
    }

    void Update()
    {

    }

    public void takeDamage(int dmg)
    {

        HP -= dmg;

        if (HP <= 0)
        {
            intactGlass.SetActive(false);
            brokenGlass.SetActive(true);
        }

    }
}
