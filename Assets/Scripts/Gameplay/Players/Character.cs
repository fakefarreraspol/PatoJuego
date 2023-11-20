using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    private Controller userInput;
    private Rigidbody2D characterRb;

    [SerializeField] private int health;
    private float speed = 20;
    [SerializeField] private float jumpForce = 3000.0f;

    private Vector2 moveVector = Vector2.zero;


    private bool shoot = false;


    private bool canJump = true;
    private bool jumped = false;





    

    private void Awake()
    {
        characterRb = GetComponent<Rigidbody2D>();
        userInput = new Controller();
    }

    private void OnEnable()
    {
        userInput.Enable();

        userInput.Player.Move.performed += OnMovementPerformed;
        userInput.Player.Move.canceled += OnMovementStopped;

        userInput.Player.Jump.performed += OnJumpPerformed;
        userInput.Player.Jump.canceled += OnJumpCancelled;

        
    }

    private void OnDisable()
    {
        userInput.Disable();

        userInput.Player.Move.performed -= OnMovementPerformed;
        userInput.Player.Move.canceled -= OnMovementStopped;

        userInput.Player.Jump.performed -= OnJumpPerformed;
        userInput.Player.Jump.canceled -= OnJumpCancelled;
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        HandleJump();
    }

    private void MoveCharacter()
    {
        Vector2 velocity = new Vector2(moveVector.x * speed, characterRb.velocity.y);
        characterRb.velocity = velocity;
    }

    private void HandleJump()
    {
        if (canJump && jumped)
        {
            Debug.Log("Jumped!!");
            characterRb.AddForce(new Vector2(0, jumpForce));
            canJump = false;
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
        // Debug.Log(moveVector);
    }

    private void OnMovementStopped(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (canJump)
        {
            jumped = true;
        }
    }

    private void OnJumpCancelled(InputAction.CallbackContext value)
    {
        jumped = false;
        canJump = true;
    }

    public Vector2 GetPlayerDir()
    {
        return moveVector;
    }
    public bool DidPlayerShoot()
    {
        return shoot;
    }

}





    


