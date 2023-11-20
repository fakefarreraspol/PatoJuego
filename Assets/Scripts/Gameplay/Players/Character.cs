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
    [SerializeField] private float jumpForce = 100.0f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpCooldown = 1.0f; // Adjust the cooldown duration as needed
    private float jumpCooldownTimer = 0.0f;
    public float groundCheckRadius = 0.2f;

    private int damage;
    private float rateOfFire;
    private float cooldown;

    protected bool isPlayerAttacking = false;
    protected bool canPlayerAttack = true;
    // Update is called once per frame
    protected Vector2 attackVector = Vector2.zero;
    private Vector2 moveVector = Vector2.zero;

    private bool shoot = false;

    private bool canJump = true;
    private bool isJumping = false;
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
        CheckGrounded();

        // Update the jump cooldown timer
        if (!canJump)
        {
            jumpCooldownTimer -= Time.fixedDeltaTime;
            if (jumpCooldownTimer <= 0.0f)
            {
                canJump = true;
                jumpCooldownTimer = 0.0f;
            }
        }
    }

    private void MoveCharacter()
    {
        Vector2 velocity = new Vector2(moveVector.x * speed, characterRb.velocity.y);
        characterRb.velocity = velocity;
        PlayerAnimations.OnSpriteChanged(velocity);

    }

    private bool IsGrounded()
    {
        // Draw a debug ray to visualize the ground check
        Debug.DrawRay(transform.position, Vector2.down * groundCheckRadius, Color.red);

        // Perform the actual ground check using Physics2D.Raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckRadius, groundLayer);

        // Return true if the ray hits something (ground)
        return hit.collider != null;
    }

    private void CheckGrounded()
    {
        if (IsGrounded())
        {
            canJump = true;
            isJumping = false;
        }
    }
    private void HandleJump()
    {
        if (canJump && jumped)
        {
            Debug.Log("Jumped!!");
            Vector2 jumpVelocity = new Vector2(0, jumpForce);
            characterRb.AddForce(jumpVelocity);

            // Update the character's state
            canJump = false;
            jumped = false;
            isJumping = true;

            // Pass the jumpVelocity to OnSpriteChanged
            PlayerAnimations.OnSpriteChanged(jumpVelocity);
            // Start the jump cooldown timer
            jumpCooldownTimer = jumpCooldown;
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (canJump)
        {
            jumped = true;
            canJump = false;
            isJumping = false;
            Debug.Log(value.ToString());

            // Start the jump cooldown timer
            jumpCooldownTimer = jumpCooldown;
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

