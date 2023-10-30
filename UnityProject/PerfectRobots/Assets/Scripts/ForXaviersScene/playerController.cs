using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("-----  Components  -----\n")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----\n")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 20)][SerializeField] float playerSpeed;
    [Range(3, 17)][SerializeField] float jumpHeight;
    [SerializeField] int InteractDist;
    [Range(-10, -40)][SerializeField] float gravityValue;
    [Range(1, 2)][SerializeField] int jumpedMax;
    [SerializeField] int jumpedTimes;


    Vector3 playerVelocity;
    bool groundedPlayer;
    Vector3 move;
    bool isShooting;
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpedTimes = 0;
        }

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 40, Color.red);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * InteractDist, Color.blue);

        if (Input.GetButton("Equip"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, InteractDist))
            {
                if (hit.collider.gameObject.CompareTag("Interactable"))
                {
                    ItemNumHolder itemHolder = hit.collider.GetComponent<ItemNumHolder>();
                    Inventory.instance.Add(hit.collider.gameObject, itemHolder.item);
                }

            }
        }
        if (Input.GetButton("Shoot") && !isShooting)
        {
            if (Inventory.instance.currentItem != null)
            {
                Interactable Using = Inventory.instance.currentItem.GetComponent<Interactable>();
                Using.useItem();
            }
        }

        if (Input.GetButton("Drop"))
        {
            if (Inventory.instance.Equipped != null)
            {
                Interactable Using = Inventory.instance.currentItem.GetComponent<Interactable>();
                Inventory.instance.RemoveItem(Inventory.instance.Equipped);
            }
        }

        // Movment
        move = (Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward);
        controller.Move(move * Time.deltaTime * playerSpeed);



        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpedMax)
        {
            playerVelocity.y = jumpHeight;
            jumpedTimes++;
        }

        // Gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}