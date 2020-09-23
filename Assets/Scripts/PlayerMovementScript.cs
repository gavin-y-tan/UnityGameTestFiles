using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovementScript : NetworkBehaviour
{

    public GameObject gun;
    public bool isSprinting;
    public bool canMove=true;

    public CharacterController controller;
    public float speed = 5f;
    public float gravity = -29.43f;
    public Transform groundCheck;
    
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpheight = .5f;
    Vector3 velocity;
    bool isGrounded;






    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //Resetting gravity

        if (isGrounded && velocity.y<0)
        {
            velocity.y = -2f;

        }
        if (isGrounded==false) //disables shooting when airborne
        {
            gun.GetComponent<MeshRenderer>().enabled = false;
        }
        //---------------------
        if (Input.GetButtonDown("Jump") && isGrounded) //jumping
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
            gun.GetComponent<MeshRenderer>().enabled = false;

        }
        
        //---------------------
        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W)) //sprinting
        {
            speed = 12f;
            gun.GetComponent<MeshRenderer>().enabled = false;
            isSprinting = true;

        }
        else
        {
            speed = 5f;
            if (isGrounded==true)
            {
                gun.GetComponent<MeshRenderer>().enabled = true;
            }    

            isSprinting = false;

        }
        //---------------------
        if (canMove != true)
        {
            speed = 0f;
        }
        else
        {

        }




        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward*z;
        controller.Move(move*speed*Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity*Time.deltaTime);
        //---------------------


    }
}
