using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;

    float timeScaleOrig;
    int enemiesRemaining;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        if(enemiesRemaining <= 0)
        {
            
        }
    }

}
