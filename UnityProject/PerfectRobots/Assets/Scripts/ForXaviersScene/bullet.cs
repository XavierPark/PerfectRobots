using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [Header("-----  Components  -----")]
    [SerializeField] Rigidbody rb;

    [Header("----- Stats -----")]
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] int damage;
    [SerializeField] List<GameObject> hitList;



    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) 
                return; 

        IDamage damageable = other.GetComponent<IDamage>();

        if (damageable != null)
        {
            if (!other.gameObject.CompareTag("Enemy"))
            {
                damageable.takeDamage(damage);
            }
        }    
        
        Destroy(gameObject);
    }
}
