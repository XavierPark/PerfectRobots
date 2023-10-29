using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour, IDamage
{
    [SerializeField] int Hp;

    int enemiesRemaining;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int dmg)
    {
        Hp -= dmg;

        if(Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void UpdateGameGoal(int amount)
    {
        enemiesRemaining += amount;

        if (enemiesRemaining <= 0)
        {
            ExitDoorCondition();
        }
    }

    public void ExitDoorCondition()
    {
        
    }
}
