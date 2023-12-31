using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Character : MonoBehaviour
{
    private Controller userInput;
    private Rigidbody2D characterRb;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;

    
    private float speed = 10;
    [SerializeField] private float jumpForce = 100.0f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpCooldown = 1.0f;
    [SerializeField] private float shootCooldown = 0.5f;

    private float jumpCooldownTimer = 0.0f;
    public float groundCheckRadius = 0.2f;

    private float shootCooldownTimer = 0.0f;
    private bool canShoot = true;
    protected bool isShooting = false;
    public Vector2 playerDir = Vector2.right;

    //private int damage;
    //private float rateOfFire;
    //private float cooldown;

    // Update is called once per frame
    protected Vector2 attackVector = Vector2.zero;
    private Vector2 moveVector = Vector2.zero;

    private bool shoot = false;

    private bool canJump = true;
    private bool isJumping = false;
    private bool jumped = false;


    private int characterHP;
    private Slider characterHPSlider;

    
    // SERVER/Client Related
    private Client client;
    private Server server;
    private UserManager ThisUser;




    //Bools to send
    private chActions playerStatus;

    private void Awake()
    {
        characterRb = GetComponent<Rigidbody2D>();
        userInput = new Controller();
        playerDir = Vector2.right;

        client = FindObjectOfType<Client>();
        server = FindObjectOfType<Server>();

        ThisUser = FindObjectOfType<UserManager>();

        //playerStatus = new chActions();

        characterHPSlider = GetComponentInChildren<Canvas>().gameObject.GetComponentInChildren<Slider>();
    }
    private void Start()
    {
        characterHP = 100;
    }

    private void OnEnable()
    {
        userInput.Enable();

        userInput.Player.Move.performed += OnMovementPerformed;
        userInput.Player.Move.canceled += OnMovementStopped;

        userInput.Player.Jump.performed += OnJumpPerformed;
        userInput.Player.Jump.canceled += OnJumpCancelled;

        userInput.Player.Shoot.performed += OnShoot;
    }

    private void OnDisable()
    {
        userInput.Disable();

        userInput.Player.Move.performed -= OnMovementPerformed;
        userInput.Player.Move.canceled -= OnMovementStopped;

        userInput.Player.Jump.performed -= OnJumpPerformed;
        userInput.Player.Jump.canceled -= OnJumpCancelled;

        userInput.Player.Shoot.performed -= OnShoot;
    }

    private void FixedUpdate()
    {
        HandleHealth();
        MoveCharacter();
        HandleJump();
        CheckGrounded();
        HandleShootingCooldown();
        //HandleShooting();

        //Debug.Log(playerDir);
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
    private void HandleHealth()
    {
        if (characterHP <= 0)
        {
            OnActionPerformed();

            Destroy(gameObject);
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

            playerStatus.jump = true;
            OnActionPerformed();
        }
        if(playerStatus.jump) { playerStatus.jump = false; OnActionPerformed(); }
        
    }

    private void HandleShootingCooldown()
    {
        if (!canShoot)
        {
            shootCooldownTimer -= Time.fixedDeltaTime;
            if (shootCooldownTimer <= 0.0f)
            {
                canShoot = true;
                shootCooldownTimer = 0.0f;
            }
        }
    }

    private void HandleShooting()
    {
        if (canShoot && shoot)
        {
            Debug.Log("Shot!");

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.GetComponent<Bullet2D>().dir = playerDir;
            canShoot = false;
            shootCooldownTimer = shootCooldown;
            shoot = false; // Reset the shooting flag
            isShooting = true;
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
    // Input System callback for shooting
    public void OnShoot(InputAction.CallbackContext value)
    {
        Debug.Log(value.ToString());
        if (canShoot)
        {
            shoot = true;
            canShoot = false;
            isShooting = false;
            Debug.Log(value.ToString());

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.GetComponent<Bullet2D>().dir = playerDir;

            // Start the jump cooldown timer
            shootCooldownTimer = shootCooldown;
            playerStatus.shoot = true;
            OnActionPerformed();
            playerStatus.shoot = false;
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        playerStatus.moving = true;
        moveVector = value.ReadValue<Vector2>();
        // Debug.Log(moveVector);
        playerDir = value.ReadValue<Vector2>().normalized;
        OnActionPerformed();
    }

    private void OnMovementStopped(InputAction.CallbackContext value)
    {
        playerStatus.moving = false;
        moveVector = Vector2.zero;
        OnActionPerformed();
    }

    private void OnJumpCancelled(InputAction.CallbackContext value)
    {
        jumped = false;
        canJump = true;
        OnActionPerformed();
    }

    public Vector2 GetPlayerDir()
    {
        return playerDir;
    }
    public bool DidPlayerShoot()
    {
        return shoot;
    }


    private void OnActionPerformed()
    {
        chInfo characterInformation = new chInfo(transform.position, GetPlayerDir(), characterHP, playerStatus);
        MessageToSend userInformation = new MessageToSend(ThisUser.userID, ThisUser.Username, MessageType.CHARACTER_INFO, "quack", characterInformation);
        string data = JsonUtility.ToJson(userInformation);
        if (client != null) client.SendData(data);
        else Debug.Log("client is null");

        if (server != null) server.SendMessageToAllClients(data);
        else Debug.Log("server is null");

    }


    public void TakeDamageCharacter(int dmg)
    {
        characterHP -= dmg;
        OnActionPerformed();
        characterHPSlider.value -= dmg*0.01f;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "deathWall")
        {
            Debug.Log("Collided");
            TakeDamageCharacter(999);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            FindObjectOfType<Character>().TakeDamageCharacter(20);
        }
    }


}

