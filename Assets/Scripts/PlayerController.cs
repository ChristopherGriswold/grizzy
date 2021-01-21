using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3.0F;
    public float strafeSpeed = 3.0f;
    private GameObject player;
    private PlayerData playerData;


    private void Awake()
    {
        
        player = this.gameObject;
    }
    private void Start()
    {
        playerData = gameObject.GetComponent<DataHandler>().playerData;
        if(playerData.playerPosition.x == 0)
        {
            player.transform.position = GameObject.Find("PlayerSpawn").transform.position;
            player.transform.rotation = GameObject.Find("PlayerSpawn").transform.rotation;
        }
        else
        {
            player.transform.position = playerData.playerPosition;
        }
        StartCoroutine("UpdatePlayerPositionData");
    }
    void Update()
    {
        if (!Application.isEditor)
        {

            transform.Translate(Vector3.forward * CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * moveSpeed);
            transform.Translate(Vector3.right * CrossPlatformInputManager.GetAxis("Horizontal") * Time.deltaTime * strafeSpeed);
        }
        else
        {
            transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed);
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * strafeSpeed);
        }
    }

    private IEnumerator UpdatePlayerPositionData()
    {
        while (true)
        {
            playerData.playerPosition = player.transform.position;
            yield return new WaitForSeconds(1f);
        }
    }

}
