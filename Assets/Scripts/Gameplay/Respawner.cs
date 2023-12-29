using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    private GameObject[] respawners;

    [SerializeField] private GameObject character;
    // Start is called before the first frame update
    private void Start()
    {
        respawners = GameObject.FindGameObjectsWithTag("respawn");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)) { RespawnCharacter(); }
    }
    public void RespawnCharacter()
    {
        int spawnerNum = Random.Range(0, respawners.Length);

        Instantiate(character, respawners[spawnerNum].transform.position, Quaternion.identity);
    }
}
