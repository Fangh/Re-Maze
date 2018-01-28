using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportZone : MonoBehaviour {

    public Transform teleportEndZone;
    public GameObject teleportFX;
    Transform player;
    GameObject fx;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(GetComponent<Collider>() == true, "Collider missing");
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnTriggerEnter()
    {
        Debug.Assert(teleportEndZone != null, "teleportEndZone missing");
        player.position = teleportEndZone.position;
        fx = GameObject.Instantiate(teleportFX, teleportEndZone.position, Quaternion.identity);
        Invoke("destroyFX", 3);
    }

    void destroyFX()
    {
        Destroy(fx);
    }
}
