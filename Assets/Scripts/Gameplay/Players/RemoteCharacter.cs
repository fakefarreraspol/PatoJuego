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
    // Start is called before the first frame update

    private void Awake()
    {
        characterRemoteHpSlider = GetComponentInChildren<Canvas>().gameObject.GetComponentInChildren<Slider>();
    }
    private void Start()
    {
        characterRemoteHP = characterRemoteMAXHP;

        InvokeRepeating("ShootBull", 0, 1);
    }
    public void UpdateRemoteCharacterPos(chInfo playerData)
    {
        transform.position = playerData.playerTransform;
    }
    private void ShootBull() 
    {
        GameObject bull = Instantiate(EnemyBullet, transform.position, Quaternion.identity);
        bull.GetComponent<Bullet2D>().dir = new Vector2(-1,0);
    }
}
