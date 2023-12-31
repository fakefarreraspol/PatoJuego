using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    private GameObject[] respawners;

    [SerializeField] private GameObject character;
    [SerializeField] private GameObject remote;
    // Start is called before the first frame update
    private void Start()
    {
        respawners = GameObject.FindGameObjectsWithTag("respawn");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)) {
            if(FindObjectOfType<Character>()==null) RespawnCharacter(); }
    }
    public void RespawnCharacter()
    {
        int spawnerNum = Random.Range(0, respawners.Length);

        Instantiate(character, respawners[spawnerNum].transform.position, Quaternion.identity);

        MessageToSend respawnedUserMessage = new MessageToSend(FindObjectOfType<UserManager>().userID, "gatinho", MessageType.RESPAWN_PLAYER, spawnerNum.ToString());
        string msg = JsonUtility.ToJson(respawnedUserMessage);
        if(FindObjectOfType<Server>()) FindObjectOfType<Server>().SendMessageToAllClients(msg);
        else FindObjectOfType<Client>().SendData(msg);
    }

    public void RespawnRemote(int respawner, int ID)
    {
        GameObject respawnedPlayer = Instantiate(remote, respawners[respawner].transform.position, Quaternion.identity);
        FindObjectOfType<ObjectManager>().AddGameObject(ID, respawnedPlayer);
    }
}
