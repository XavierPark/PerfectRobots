using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player; //test

    //menus
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuMenu;
    [SerializeField] GameObject reticlePause;

    float timescaleOrig;
    int enemiesRemaining;

    //door stuff - Dami
    EndDoor endDoor;
    [SerializeField] GameObject door;
    FinishLine finish;
    [SerializeField] Collider box;
    static private int floorLevelMax = 1;
    int currFloorFinish;

    public bool isPaused;

    void Awake()
    {
        Instance = this;
        endDoor = door.GetComponent<EndDoor>();
        finish = box.GetComponent<FinishLine>();
        timescaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel") && menuActive == null)
        {
            isPaused = !isPaused;
            menuPause.SetActive(isPaused);
            reticlePause.SetActive(!isPaused);

        }
    }
    public void UpdateGameGoal(int amount)
    {
        enemiesRemaining += amount;

        if(enemiesRemaining <= 0)
        {
            StartCoroutine(endDoor.OpenDoors());
        }
    }

    public void ExitDoorCondition()
    {
        currFloorFinish++;
        if (currFloorFinish == floorLevelMax)
        {
            YouWin();
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timescaleOrig;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void YouWin()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
        currFloorFinish = 0;
    }
}
