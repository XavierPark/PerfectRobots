using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldStats : MonoBehaviour, IDamage
{
    [Header("===== Components =====")]
    [SerializeField] MeshRenderer model;
    [SerializeField] AudioSource aud;

    [Header("===== Stats =====")]
    [Range(1, 5)][SerializeField] int shieldHp;

    [Header("===== Audio =====")]
    [SerializeField] AudioClip audDamage;
    [Range(0, 1)][SerializeField] float audDamageVol;
    [SerializeField] AudioClip audShieldOn;
    [Range(0, 1)][SerializeField] float audShieldOnVol;

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
            aud.PlayOneShot(audShieldOn, audShieldOnVol);
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
        aud.PlayOneShot(audDamage, audDamageVol);
        if (shieldHp <= 0)
        {
            shieldDead = true;
            shieldOff();
        }
    }
}
