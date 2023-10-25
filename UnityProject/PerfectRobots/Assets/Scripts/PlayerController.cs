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

    private Vector3 move;
    private Vector3 playerVelocity;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxis("Horizontal") * transform.right +
              Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
