using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;

    int enemiesRemaining;

    //door stuff - Dami
    EndDoor endDoor;
    [SerializeField]GameObject door;
    FinishLine finish;
    static private int floorLevelMax = 1;
    int currFloorFinish;

    public bool isPaused;

    void Awake()
    {
        instance = this;
        endDoor = door.GetComponent<EndDoor>();
        player = GameObject.FindWithTag("Player");
    }

    public void UpdateGameGoal(int amount)
    {
        enemiesRemaining += amount;

        if(enemiesRemaining <= 0)
        {
            StartCoroutine(endDoor.OpenDoors());
            ExitDoorCondition();
        }
    }

    public void ExitDoorCondition()
    {
        if(finish.playerInArea == true)
        {
            currFloorFinish++;
            if(currFloorFinish == floorLevelMax)
            {
                YouWin();
            }
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void YouWin()
    {
        statePause();
    }
}
