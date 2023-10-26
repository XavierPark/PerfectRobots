using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(-10, -40)][SerializeField] float gravityValue;
    [Range(2, 8)][SerializeField] float playerSpeed;

    [Header("----- Gun Stats -----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] int shootRate;

    private Vector3 move;
    private Vector3 playerVelocity;
    bool isShooting;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        if (Input.GetButton("Shoot") && !isShooting)
            StartCoroutine(shoot());

        move = Input.GetAxis("Horizontal") * transform.right +
              Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && damageable != null)
            {
                damageable.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
