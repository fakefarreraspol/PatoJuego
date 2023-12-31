using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class RemoteCharacter : MonoBehaviour
{
    public float characterRemoteMAXHP;
    private float characterRemoteHP;
    private Slider characterRemoteHpSlider;
    public GameObject EnemyBullet;
    private Animator characterRemoteAnim;
    private SpriteRenderer characterRemoteSpriteRenderer;
    // Start is called before the first frame update
    private float speed = 10;
    Vector2 lastDir = Vector2.right;
    private float jumpForce = 100.0f;

    private bool isMoving;
    private bool jumped = false;
    private Rigidbody2D characterRemoteRigidbody;
    private void Awake()
    {
        characterRemoteHpSlider = GetComponentInChildren<Canvas>().gameObject.GetComponentInChildren<Slider>();
        characterRemoteAnim = GetComponent<Animator>();
        characterRemoteSpriteRenderer = GetComponent<SpriteRenderer>();
        characterRemoteRigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        characterRemoteHP = characterRemoteMAXHP;

    }
    private void FixedUpdate()
    {
        if (isMoving) 
        {
            Vector2 velocity = new Vector2(lastDir.x * speed, characterRemoteRigidbody.velocity.y);
            characterRemoteRigidbody.velocity = velocity;
        }
        else characterRemoteRigidbody.velocity = new Vector2(0, characterRemoteRigidbody.velocity.y);
    }
    public void UpdateRemoteCharacter(MessageToSend userData)
    {
        chInfo playerData = userData.UserCharacterInfo;
        UpdatePos(playerData);
        UpdateAnimations(playerData);
        lastDir = playerData.playerDirection;
        if (playerData.characterActions.shoot) ShootBull(playerData.playerDirection);

        HandleHealth(playerData.healthPoints, userData.ID);

        jumped = playerData.characterActions.jump;
        if(jumped)
        {
            HandleRemoteJump();
        }
    }
    private void ShootBull(Vector2 dir) 
    {
        GameObject bull = Instantiate(EnemyBullet, transform.position, Quaternion.identity);
        bull.GetComponent<Bullet2D>().dir = dir;
    }
    private void HandleRemoteJump()
    {
        Vector2 jumpVelocity = new Vector2(0, jumpForce);
        characterRemoteRigidbody.AddForce(jumpVelocity);
        jumped = false;
    }
    private void HandleHealth(int hp, int ID)
    {
        if (hp <= 0)
        {
            FindObjectOfType<ObjectManager>().RemoveGameObject(ID);
            DestroyRemotePlayer();
        }
        
        characterRemoteHpSlider.value = hp * 0.01f;
    }
    private void UpdatePos(chInfo playerData)
    {
        transform.position = playerData.playerTransform;
        isMoving = playerData.characterActions.moving;


    }
    private void UpdateAnimations(chInfo playerData)
    {
        if(playerData.characterActions.moving)
        {
            characterRemoteAnim.SetBool("isMoving", true);
        }
        else characterRemoteAnim.SetBool("isMoving", false);
        
        if(playerData.characterActions.jump)
        {
            characterRemoteAnim.SetBool("Jump", true);
        }
        else characterRemoteAnim.SetBool("Jump", false);


        if (playerData.playerDirection.x > 0)
            characterRemoteSpriteRenderer.flipX = false;

        else if (playerData.playerDirection.x < 0) characterRemoteSpriteRenderer.flipX = true;
    }


    private void DestroyRemotePlayer()
    {
        Destroy(gameObject);
    }
}
