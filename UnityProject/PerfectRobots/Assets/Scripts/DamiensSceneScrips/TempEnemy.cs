using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour, IDamage
{
    [SerializeField] int Hp;
    [SerializeField] Renderer model;

    void Start()
    {
        GameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int dmg)
    {
        StartCoroutine(FlashRed());
        Hp -= dmg;

        if (Hp <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.UpdateGameGoal(-1);
        }
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
