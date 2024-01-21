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
    private bool hasWeapon = false;
    [SerializeField] private GameObject weapon;
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

    //raycast
    public float raycastDistance = 0.1f;
    public Transform RaycastPos01;
    public Transform RaycastPos02;

    //Bools to send
    private chActions playerStatus;


    [SerializeField]private GameObject turkey;
    private void Awake()
    {
        characterRb = GetComponent<Rigidbody2D>();
        userInput = new Controller();
        playerDir = Vector2.right;

        client = FindObjectOfType<Client>();
        server = FindObjectOfType<Server>();

        ThisUser = FindObjectOfType<UserManager>();

        //playerStatus = new chActions();
        weapon.SetActive(false);
        characterHPSlider = GetComponentInChildren<Canvas>().gameObject.GetComponentInChildren<Slider>();
    }
    private void Start()
    {
        characterHP = 100;
        playerStatus.hasWeapon = false;
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
        moveWeapon();
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
        CheckForObstacles();
        HandleTroll();
    }
    private void HandleTroll()
    {
        if (characterRb.velocity == Vector2.zero)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                playerStatus.makingDead = true;
                OnActionPerformed();
                PlayerAnimations.Onded(true);
                turkey.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {
            playerStatus.makingDead = false;
            PlayerAnimations.Onded(false);
            turkey.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    private void moveWeapon()
    {
        if (GetPlayerDir().x > 0)
        {
            weapon.GetComponent<SpriteRenderer>().flipX = false;
            weapon.transform.localPosition = new Vector2(0.2f, weapon.transform.localPosition.y);
            shootPoint.transform.localPosition = new Vector2(0.45f, shootPoint.transform.localPosition.y);
        }
        else
        {
            weapon.GetComponent<SpriteRenderer>().flipX = true;
            weapon.transform.localPosition = new Vector2(-0.2f,weapon.transform.localPosition.y);
            shootPoint.transform.localPosition = new Vector2(-0.45f, shootPoint.transform.localPosition.y);
        }
    }
    private void HandleHealth()
    {
        if (characterHP <= 0)
        {
            OnActionPerformed();
            FindObjectOfType<winlose>().playerDied();

            Vector3 pos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            GameObject wturkey = Instantiate(turkey, pos, Quaternion.identity);
            wturkey.GetComponent<SpriteRenderer>().enabled = true;


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
    private void CheckForObstacles()
    {
        // Cast rays from the sides of the player
        RaycastHit2D leftHit = Physics2D.Raycast(RaycastPos01.transform.position - new Vector3(0.5f, 0f, 0f), Vector2.left, raycastDistance);
        RaycastHit2D rightHit = Physics2D.Raycast(RaycastPos01.transform.position + new Vector3(0.5f, 0f, 0f), Vector2.right, raycastDistance);

        RaycastHit2D leftHit2 = Physics2D.Raycast(RaycastPos02.transform.position - new Vector3(0.5f, 0f, 0f), Vector2.left, raycastDistance);
        RaycastHit2D rightHit2 = Physics2D.Raycast(RaycastPos02.transform.position + new Vector3(0.5f, 0f, 0f), Vector2.right, raycastDistance);

        // Check if there are obstacles on the sides
        bool leftObstacle = leftHit.collider != null && leftHit.collider.CompareTag("tile");
        bool rightObstacle = rightHit.collider != null && rightHit.collider.CompareTag("tile");

        bool leftObstacle2 = leftHit2.collider != null && leftHit2.collider.CompareTag("tile");
        bool rightObstacle2 = rightHit2.collider != null && rightHit2.collider.CompareTag("tile");

        // Adjust the player's behavior based on obstacles
        if ((leftObstacle && !rightObstacle) || (leftObstacle2 && !rightObstacle2))
        {
            // Obstacle on the left side
            if (characterRb.velocity.x < 0)
            {
                characterRb.velocity = new Vector2(0f, characterRb.velocity.y);
            }
            
        }
        else if ((rightObstacle && !leftObstacle) || (rightObstacle2 && !leftObstacle2))
        {
            // Obstacle on the right side
            if (characterRb.velocity.x > 0)
            {
                characterRb.velocity = new Vector2(0f, characterRb.velocity.y);
            }
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
        if (canShoot && hasWeapon)
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
        if(other.gameObject.tag == "weapon")
        {
            if (hasWeapon == false)
            {
                hasWeapon = true;
                playerStatus.hasWeapon = true;
                weapon.SetActive(true);
                OnActionPerformed();
            }
             
        }
    }


}

