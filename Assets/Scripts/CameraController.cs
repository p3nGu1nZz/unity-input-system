using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public static CameraController cam;
    private Camera cam_;

    //public Transform player;
    public Transform CameraPosition;
    public float sensitivity = 3;
    public float maxUpAngle = 80;
    public float maxDownAngle = -80;

    [HideInInspector]
    public float mouseX, mouseY;
    private Vector2 direction;
    private PlayerController player;

    private void Awake()
    {
        cam = this;
        cam_ = this.GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player = FindObjectOfType<PlayerController>();

    }

    private float rotX = 0.0f, rotY = 0.0f;
    [HideInInspector]
    public float rotZ = 0.0f;

    private void Update()
    {
        // Mouse input
        //mouseX = Input.GetAxis("Mouse X") * sensitivity;
        //mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // TODO need to add mouse input for looking too.

        // BUG uses the wrong stick to look.

        //new input system
        direction = player.Controls.Player.Look.ReadValue<Vector2>();

        Debug.Log(direction);

        if (direction.x > 0 || direction.x < 0 || direction.y > 0 || direction.y < 0)
        {
            //player.Move(direction);

            mouseX = direction.x * sensitivity;
            mouseY = direction.y * sensitivity;

            // need to implement
            // player.Look(direction)
        }

        // Calculations
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, maxDownAngle, maxUpAngle);
        rotY += mouseX;

        // Placing values
        transform.localRotation = Quaternion.Euler(rotX, rotY, rotZ);
        player.transform.Rotate(Vector3.up * mouseX);
        transform.position = CameraPosition.position;
    }

    /**
     * This can be called anytime during the update method. This function
     * will wait until the end of the frame, while adding a small random 
     * increment or decrement. 
     */
    public void ShakeCamera(float magnitude, float duration)
    {
        StartCoroutine(ICameraShaker(magnitude, duration));
    }

    /**
     * Runs a loop to randomly shake the camera z axis every frame for the 
     * given duration. Can be called multiple times. be careful young skywalk
     */
    private IEnumerator ICameraShaker(float mag, float dur)
    {
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        for (float t = 0.0f; t <= dur; t += Time.deltaTime)
        {
            rotZ = Random.Range(-mag, mag) * (t / dur - 1.0f);
            yield return wfeof;
        }
        rotZ = 0.0f;
    }
}
