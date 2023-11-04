using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    private Controller userInput;

    [SerializeField] private int health;
    private float speed = 20;
    private int damage;
    private float rateOfFire;
    private float cooldown;

    protected bool isPlayerAttacking = false;
    protected bool canPlayerAttack = true;
    // Update is called once per frame
    protected Vector2 attackVector = Vector2.zero;



    private Rigidbody2D characterRb;
    private Vector2 moveVector = Vector2.zero;

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
        userInput.Player.Jump.performed += OnJumpCancelled;
    }
    private void OnDisable()
    {
        userInput.Disable();
        
        userInput.Player.Move.performed -= OnMovementPerformed;
        userInput.Player.Move.canceled -= OnMovementStopped;

        userInput.Player.Jump.performed -= OnJumpPerformed;
        userInput.Player.Jump.performed -= OnJumpCancelled;
    }
    
    
    // Update is called once per frame
    protected void FixedUpdate()
    {
        Vector2 velocity = new Vector2(moveVector.x * speed, 0);
        characterRb.velocity = velocity;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
        Debug.Log(moveVector);
        //PlayerAnimations.OnSpriteChanged(moveVector);
    }

    private void OnMovementStopped(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
        //PlayerAnimations.OnSpriteChanged(moveVector);
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        Debug.Log(value.ToString());
    }

    private void OnJumpCancelled(InputAction.CallbackContext value)
    {
        Debug.Log(value.ToString());
    }
}
