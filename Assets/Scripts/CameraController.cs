using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float cameraDistance;
    public float cameraHeight;

    // Use this for initialization
    void Start()
    {
    }

    void LateUpdate()
    {
        transform.position = player.transform.position - player.transform.forward * cameraDistance;
        transform.LookAt(player.transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y + cameraHeight, transform.position.z);
    }
}
