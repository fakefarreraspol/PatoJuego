using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeCharacterRemote : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bullet;

    private SpriteRenderer sprRenderer;

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }
    public void UpdateRemoteCharacterPos(fakePlayerData playerData)
    {
        if(playerData.Health <= 0)
        {
            Destroy(gameObject); return;
        }
        transform.position = playerData.playerTransform;
        if (playerData.bulletShot)
        {
            GameObject bull = Instantiate(bullet, transform.position, Quaternion.identity);
            bull.GetComponent<Bullet2D>().dir = playerData.playerDirection;
        }

        HandleAnimations(playerData.playerDirection);
    }

    private void HandleAnimations(Vector2 pDir)
    {
        if (pDir.x < 0)
        {
            sprRenderer.flipX = true;
        }
        else sprRenderer.flipX = false;
    }
}
