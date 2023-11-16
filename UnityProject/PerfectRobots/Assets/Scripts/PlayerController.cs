using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage //Added this since you have take damage added -Dami
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform shootPos2;


    [Header("----- Player Stats -----")]
    [Range(1, 10)]public int HP;
    [Range(0, 10)][SerializeField] int Shield;
    [SerializeField] AudioSource aud;
    [Range(2, 8)][SerializeField] float playerSpeed;
    [Range(8, 30)][SerializeField] float jumpHeight;
    [Range(3, 6)][SerializeField] int sprintMod;
    [Range(1, 4)][SerializeField] int jumpsMax;
    [Range(-10, -40)][SerializeField] float gravityValue;
    

    [Header("----- Gun Stats -----")]
    [SerializeField] List<GunStats> gunList = new List<GunStats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject gunModel2;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate; //Changed to float so we can have faster gunfire - Dami
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bullet2;
    [SerializeField] float reloadTime;

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
    public int HPOrig;
    int selectedGun;
    Transform gunPosTransform;
    Transform gunOrgPosTransform;
    int ShieldOrig;
    

    void Start()
    {
        changeGun();
        HPOrig = HP;
        ShieldOrig = Shield;
        SpawnPlayer();
        gunPosTransform = gunModel.transform;
        Transform gunOrgPosTransform = gunModel.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            Movement();
        }

        if (gunList.Count > 0)
        {
            selectGun();
            if (Input.GetButton("Shoot") && !isShooting)
            { StartCoroutine(shoot()); }
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
        RaycastHit hit;
        //Debug.Log("shoot() called;");
        //Debug.Log("There are " + gunList[selectedGun].ammoInMagCurr + " bullets left in " + gunList[selectedGun].name  + ";");
        if (gunList[selectedGun].ammoInMagCurr > 0)
        {
            isShooting = true;
            gunList[selectedGun].ammoInMagCurr--;
            
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                //Commenting this out to see if thats why my glass will not work -Dami
                if (!gunList[selectedGun].Electric)
                {
                    Instantiate(bullet, shootPos.position, transform.rotation);
                }
                else
                {
                    Instantiate(bullet2, shootPos2.position, transform.rotation);
                }
                //Debug.Log("Instantiate(bullet, shootPos.postion, transform.rotation); called;");
                IDamage damageable = hit.collider.GetComponent<IDamage>();
                EnemyAI Enemy = hit.collider.GetComponent<EnemyAI>();

                if (hit.transform != transform && damageable != null)
                {
                    if (Enemy.robotType && gunList[selectedGun].Electric)
                    {
                        damageable.takeDamage(shootDamage * 2);
                    }
                    else
                    {
                        damageable.takeDamage(shootDamage);
                    }
                }
            }
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
        else
        {
            isShooting = true;
            yield return new WaitForSeconds(reloadTime);
            if (!gunList[selectedGun].Electric)
            {
                if (GameManager.Instance.norAmmoCurr >= gunList[selectedGun].ammoMagMax)
                {
                    Mathf.Clamp(GameManager.Instance.norAmmoCurr -= gunList[selectedGun].ammoMagMax, 0, 1000);
                    gunList[selectedGun].ammoInMagCurr = gunList[selectedGun].ammoMagMax;
                }
                else
                {
                    gunList[selectedGun].ammoInMagCurr = GameManager.Instance.norAmmoCurr;
                    GameManager.Instance.norAmmoCurr = 0;
                }
            }
            else
            {
                if (GameManager.Instance.eleAmmoCurr >= gunList[selectedGun].ammoMagMax)
                {
                    Mathf.Clamp(GameManager.Instance.eleAmmoCurr -= gunList[selectedGun].ammoMagMax, 0, 1000);
                    gunList[selectedGun].ammoInMagCurr = gunList[selectedGun].ammoMagMax;
                }
                else
                {
                    gunList[selectedGun].ammoInMagCurr = GameManager.Instance.eleAmmoCurr;
                    GameManager.Instance.eleAmmoCurr = 0;
                }
            }
            isShooting = false;
        }
        isShooting = true;
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

    void selectGun()
    {
        float MouseScrollWheelOrg = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            Mathf.Clamp(selectedGun++, 0, gunList.Count);
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            Mathf.Clamp(selectedGun--, 0, gunList.Count);
            changeGun();
        }

        if (Input.GetAxis("Mouse ScrollWheel") != MouseScrollWheelOrg)
        {
            Debug.Log("Mouse scroll wheel value is " + Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    void changeGun()
    {

        if (selectedGun == 1)
        {
            gunModel2.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            //    Vector3 gunPosCurrPos = gunOrgPosTransform.position;
            //    gunPosCurrPos.x += 1;
            //    gunPosTransform.position = gunPosCurrPos;
            gunModel.GetComponent<MeshFilter>().sharedMesh = null;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = null;
            gunModel2.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
            gunModel2.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
        }
        else if (selectedGun == 0)
        {
            gunModel.transform.localScale = new Vector3(50, 50, 50);
            //Vector3 gunPosCurrPos = gunPosTransform.position;
            //gunPosTransform.position = gunOrgPosTransform.position;
            gunModel2.GetComponent<MeshFilter>().sharedMesh = null;
            gunModel2.GetComponent<MeshRenderer>().sharedMaterial = null;
            gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
        }
        Debug.Log("int selected Gun = " + selectedGun + " and current gun name is " + gunList[selectedGun].name);

        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;


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
