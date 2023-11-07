using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player; //test
    public List<GameObject> groundObjectPosList;
    public GameObject[] GBtempArray;


    //menus
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuMain;
    [SerializeField] GameObject reticlePause;
    [SerializeField] TMP_Text enemyCount;
    [SerializeField] Image exitBG;
    [SerializeField] GameObject playerDmgScreen;

    public Image playerHpBar;
    [SerializeField] TMP_Text getToTheChopper;
    public bool endGame;
    public bool startGame = false;

    float timescaleOrig;
    int enemiesRemaining;
    public GameObject playerSpawnPos;
    public PlayerController playerScript;


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
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of GroundObjectController found!");
        }
        Instance = this;
        endDoor = door.GetComponent<EndDoor>();
        finish = box.GetComponent<FinishLine>();
        timescaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");        
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindWithTag("Respawn");
       
    }

    void Start()
    {
       
        //GBtempArray = GameObject.FindGameObjectsWithTag("LazerBlasterSpPos");
        //groundObjectPosList.AddRange(GBtempArray);
        //GBtempArray = GameObject.FindGameObjectsWithTag("AmmoSpPos");
        //groundObjectPosList.AddRange(GBtempArray);

        foreach (GameObject obj in groundObjectPosList)
        {
            if (obj.CompareTag("LazerBlasterSpPos"))
            {
                Vector3 spPos = obj.transform.position;
                Quaternion spRot = obj.transform.rotation;
                instantiateGroundObject(Inventory.instance.LazerBlasterGO, spPos, spRot);
            }
            else if (obj.CompareTag("AmmoSpPos"))
            {
                Vector3 spPos = obj.transform.position;
                Quaternion spRot = obj.transform.rotation;
                instantiateGroundObject(Inventory.instance.AmmoGO, spPos, spRot);
            }
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel") && menuActive == null)
        {
            statePause();
            menuActive = menuPause;
            menuPause.SetActive(isPaused);
            //reticlePause.SetActive(!isPaused);

        }
        if (startGame == false)
        {
            Begin();
        }
    }
    public void UpdateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemyCount.text = enemiesRemaining.ToString("0");

        if(enemiesRemaining <= 0)
        {
            StartCoroutine(endDoor.OpenDoors());
            //Door having stroke
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

    public void mainMenu()
    {
        statePause();
        menuActive = menuMain;
        menuActive.SetActive(true);
    }

    public void YouWin()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
        currFloorFinish = 0;
    }

    public void YouLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
        currFloorFinish = 0;
    }

    public void instantiateGroundObject(GameObject obj, Vector3 spPos, Quaternion spRot)
    {
        Instantiate(obj, spPos, spRot);
    }

    public void itemUsedGBC(GameObject calledBy, SphereCollider collider)
    {
        if (calledBy.transform.parent == null)
        {
            Destroy(gameObject);
        }
        else
        {
            collider.enabled = false;
        }
    }

    public void Gravitate(GameObject calledBy, float floatSpeed, Vector3 initialPosition, float rotateSpeed)
    {
        if (transform.parent == null)
        {
            float floatOffset = Mathf.Sin(Time.time * floatSpeed) * 0.1f;
            Vector3 newPosition = initialPosition + new Vector3(0, floatOffset, 0);
            calledBy.transform.position = newPosition;
            calledBy.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
    }

    public IEnumerator PlayerFlashDamage()
    {
        playerDmgScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDmgScreen.SetActive(false);
    }

    public void Begin()
    {
        menuActive = menuMain;
        menuActive.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
