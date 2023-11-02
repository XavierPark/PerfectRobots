using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage //Added this since you have take damage added -Dami
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Transform shootPos;

    [Header("----- Player Stats -----")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(2, 8)][SerializeField] float playerSpeed;
    [Range(8, 30)][SerializeField] float jumpHeight;
    [Range(1, 4)][SerializeField] int jumpsMax;
    [Range(-10, -40)][SerializeField] float gravityValue;
    

    [Header("----- Gun Stats -----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate; //Changed to float so we can have faster gunfire - Dami
    [SerializeField] GameObject bullet;
    
    
    private Vector3 move;
    private Vector3 playerVelocity;
    bool isShooting;
    private bool groundedPlayer;
    private int jumpTimes;
    int HPOrig;

    void Start()
    {
        HPOrig = HP;
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {

        Movement();
    }

    void Movement()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist);

        if (Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }



        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpTimes = 0;
        }

        move = Input.GetAxis("Horizontal") * transform.right +
               Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * Time.deltaTime * playerSpeed);


        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpTimes < jumpsMax)
        {
            playerVelocity.y = jumpHeight;
            jumpTimes++;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    IEnumerator shoot()
    {
        isShooting = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            //Commenting this out to see if thats why my glass will not work -Dami
            Instantiate(bullet, shootPos.position, transform.rotation);
            IDamage damageable = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && damageable != null)
            {
                damageable.takeDamage(shootDamage);

            }
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(GameManager.Instance.PlayerFlashDamage());

        if (HP <= 0)
        {
            GameManager.Instance.YouLose();
        }
    }

    public void SpawnPlayer()
    {
        controller.enabled = false;
        HP = HPOrig;
        updatePlayerUI();
        transform.position = GameManager.Instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void updatePlayerUI()
    {
        GameManager.Instance.playerHpBar.fillAmount = (float)HP / HPOrig;
    }
}
