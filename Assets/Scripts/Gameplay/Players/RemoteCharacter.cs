using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    private bool isMoving;
    private void Awake()
    {
        characterRemoteHpSlider = GetComponentInChildren<Canvas>().gameObject.GetComponentInChildren<Slider>();
        characterRemoteAnim = GetComponent<Animator>();
        characterRemoteSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        characterRemoteHP = characterRemoteMAXHP;

    }
    public void UpdateRemoteCharacter(chInfo playerData)
    {
        UpdatePos(playerData);
        UpdateAnimations(playerData);
    }
    private void ShootBull() 
    {
        GameObject bull = Instantiate(EnemyBullet, transform.position, Quaternion.identity);
        bull.GetComponent<Bullet2D>().dir = new Vector2(-1,0);
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
}
