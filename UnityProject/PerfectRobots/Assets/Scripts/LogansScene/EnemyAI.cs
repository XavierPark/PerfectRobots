using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] SkinnedMeshRenderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [Header("----- Enemy Stats -----")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 20)][SerializeField] int playerFaceSpeed;

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;


    Vector3 playerDir;
    bool isShooting;
    bool playerInRange;
    Material material;

    void Start()
    {
        GameManager.Instance.UpdateGameGoal(1);
    }

    void Update()
    {
        if (playerInRange)
        {
            playerDir = GameManager.Instance.player.transform.position - transform.position;

            if (!isShooting)
            {
                StartCoroutine(shoot());
            }

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                faceTarget();
            }

            agent.SetDestination(GameManager.Instance.player.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        //using transform.rotation will shoot the bullet wherever the enemy is pointing
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());
        agent.SetDestination(GameManager.Instance.player.transform.position);

        if (HP <= 0)
        {
            GameManager.Instance.UpdateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        material = model.material;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material = material;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
}
