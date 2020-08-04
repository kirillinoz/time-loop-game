﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;

    float verticalMove = 0f;

    bool jump = false;

    bool inDoor = false;

    Transform linkedDoor;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        verticalMove = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if(Input.GetButtonDown("Vertical") && verticalMove > 0)
        {
            if (inDoor == true)
            {
                if(linkedDoor != null)
                {
                    transform.position = linkedDoor.position;
                    verticalMove = 0;
                }
            }
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Door"))
        {
            inDoor = true;
            Debug.Log(inDoor);
            Transform doorParent = collision.gameObject.transform.parent;
            for (int i = 0; i < doorParent.childCount; i++)
            {
                if (doorParent.GetChild(i).gameObject != collision.gameObject)
                {
                    if (doorParent.GetChild(i).gameObject.name.Equals(collision.gameObject.name))
                    {
                        linkedDoor = doorParent.GetChild(i);
                        Debug.Log(linkedDoor.position);
                    }
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Door"))
        {
            inDoor = false;
            Debug.Log(inDoor);
            linkedDoor = null;
        }
    }
}
