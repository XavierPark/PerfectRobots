using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObjectController : MonoBehaviour
{
    public static GroundObjectController instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of GroundObjectController found!");
        }
        instance = this;
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