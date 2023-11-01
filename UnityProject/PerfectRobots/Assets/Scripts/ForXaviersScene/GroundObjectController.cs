using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObjectController : MonoBehaviour
{
    public static GroundObjectController gbController;
    public List<GameObject> groundObjectPosList;
    public GameObject[] GBtempArray;



    void Awake()
    {
        if (gbController != null)
        {
            Debug.LogWarning("More than one instance of GroundObjectController found!");
        }
        gbController = this;
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

    public void instantiateGroundObject(GameObject obj,Vector3 spPos, Quaternion spRot)
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