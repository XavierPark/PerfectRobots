using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage //Added this since you have take damage added -Dami
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Transform shootPos;
    [SerializeField] AudioSource aud;

    [Header("----- Player Stats -----")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(0, 10)][SerializeField] int Shield;
    [Range(2, 8)][SerializeField] float playerSpeed;
    [Range(8, 30)][SerializeField] float jumpHeight;
    [Range(3, 6)][SerializeField] int sprintMod;
    [Range(1, 4)][SerializeField] int jumpsMax;
    [Range(-10, -40)][SerializeField] float gravityValue;
    

    [Header("----- Gun Stats -----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate; //Changed to float so we can have faster gunfire - Dami
    [SerializeField] GameObject bullet;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audDamage;
    [Range(0, 1)][SerializeField] float audDamageVol;
    [SerializeField] AudioClip audLazer;
    [Range(0, 1)][SerializeField] float audLazerVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;

    private Vector3 move;
    private Vector3 playerVelocity;
    bool isShooting;
    bool isPlayingSteps;
    bool isSprinting;
    private bool groundedPlayer;
    private int jumpTimes;
    int HPOrig;
    int ShieldOrig;
    

    void Start()
    {
        HPOrig = HP;
        ShieldOrig = Shield;
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            Movement();
        }
    }

    void Movement()
    {
        
         sprint();
        
       
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist);

        if (Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && move.normalized.magnitude > 0.3f && !isPlayingSteps)
        {
            StartCoroutine(playSteps());
        }

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
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            playerVelocity.y = jumpHeight;
            jumpTimes++;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator playSteps()
    {
        isPlayingSteps = true;

        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

        yield return new WaitForSeconds(0.4f);

        isPlayingSteps = false;
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            
            isSprinting = true;
            playerSpeed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            
            isSprinting = false;
            playerSpeed /= sprintMod;
        }
    }


    IEnumerator shoot()
    {
        isShooting = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            Instantiate(bullet, shootPos.position, transform.rotation);
            aud.PlayOneShot(audLazer, audLazerVol);
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
        if (Shield == 0)
        {
            HP -= amount;
            Debug.Log(HP);
            updatePlayerUI();
            aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);
            StartCoroutine(GameManager.Instance.PlayerFlashDamage());
        }
        else
        {
             Shield -= amount;
            updateShieldUI();
        }
        

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
        updateShieldUI();
        transform.position = GameManager.Instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void updatePlayerUI()
    {
         //Debug.Log("No!");
         GameManager.Instance.playerHpBar.fillAmount = (float)HP / HPOrig;
    }

    public void updateShieldUI()
    {  
         GameManager.Instance.playerShieldBar.fillAmount = (float)Shield / ShieldOrig;
    }

}
