using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of GroundObjectController found!");
        }
        Instance = this;
        endDoor = door.GetComponent<EndDoor>();
        finish = box.GetComponent<FinishLine>();
        timescaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        GBtempArray = GameObject.FindGameObjectsWithTag("LazerBlasterSpPos");
        groundObjectPosList.AddRange(GBtempArray);
        GBtempArray = GameObject.FindGameObjectsWithTag("AmmoSpPos");
        groundObjectPosList.AddRange(GBtempArray);

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
}
