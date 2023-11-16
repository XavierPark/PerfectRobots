using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldStats : MonoBehaviour, IDamage
{
    [Header("===== Components =====")]
    [SerializeField] MeshRenderer model;
    [SerializeField] GameObject shieldPos;

    [SerializeField] GameObject shieldPos;
    [SerializeField] AudioSource aud;

    [Header("===== Stats =====")]
    [Range(1, 5)][SerializeField] int shieldHp;

    bool playerInShieldRange;
    bool shieldDead = false;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(playerInShieldRange && !shieldDead)
        {
            shieldOn();
        }
        else
        {
            shieldOff();
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInShieldRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInShieldRange = false;
        }
    }

    void shieldOn()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    void shieldOff()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void takeDamage(int amount)
    {
        shieldHp -= amount;
        if(shieldHp <= 0)
        {
            shieldDead = true;
            shieldOff();
        }
    }
}
