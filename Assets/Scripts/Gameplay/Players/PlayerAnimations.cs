using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using System;

public class PlayerAnimations : MonoBehaviour
{
    private Sprite playerSprite;
    private SpriteRenderer sprRenderer;
    private Animator animator;

    public static Action<Vector2> OnSpriteChanged;
    public static Action<bool> Onded;

    private void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        OnSpriteChanged += ChangeSpriteDirection;
        OnSpriteChanged += ChangeAnimation;
        Onded += makingDead;
    }
    private void OnDisable()
    {
        OnSpriteChanged -= ChangeSpriteDirection;
        OnSpriteChanged -= ChangeAnimation;
        Onded -= makingDead;
    }



    private void ChangeAnimation(Vector2 dir)
    {
        if (dir.x != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else animator.SetBool("isMoving", false);

        if (dir.y != 0)
        {
            animator.SetBool("Jump", true);
        }
        else animator.SetBool("Jump", false);
    }

    private void ChangeSpriteDirection(Vector2 dir)
    {
        if (dir.x > 0)
            sprRenderer.flipX = false;

        else if (dir.x < 0) sprRenderer.flipX = true;
    }


    private void makingDead(bool isit)
    {
        if (isit) sprRenderer.color = new Color(1f, 1f, 1f, 0f);
        else sprRenderer.color = new Color(1f, 1f, 1f, 1f);

    }
}
