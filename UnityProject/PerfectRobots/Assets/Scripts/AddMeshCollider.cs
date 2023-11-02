using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddMeshCollider : MonoBehaviour
{
    [SerializeField] GameObject Level;
    // Start is called before the first frame update
    void Start()
    {
        MeshCollider[] mesh = Level.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider c in mesh)
        {

            MeshCollider meshCollider = c.AddComponent<MeshCollider>();
            meshCollider.convex = true;
            meshCollider.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
