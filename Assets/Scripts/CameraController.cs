using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(PlayerController))]
public class CameraController : MonoBehaviour
{
    public static CameraController cam;
    private Camera cam_;
    private PlayerController player;

    public Transform CameraPosition;
    public float sensitivity = 5;
    public float maxRotX = 80;
    public float minRotX = -80;

    [HideInInspector]
    public float lookX, lookY;
    private Vector2 direction;

    private float rotX = 0.0f, rotY = 0.0f;
    [HideInInspector]
    public float rotZ = 0.0f;

    /**
     * Grab an instance of the camera which should be attached to this game object
     * Then we need to lock the cursor to the center of the screeb
     */
    private void Awake()
    {
        cam = this;
        cam_ = this.GetComponent<Camera>();

        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        direction = player.Controls.Player.Look.ReadValue<Vector2>();
        this.Look(direction);
    }

    /**
     * Calculates the new direction for the player to look at. This is independent of 
     * the player states. We assume that we always want the player to be looking around
     * unless this state is disabled for a cut scene or something.
     */
    public void Look(Vector3 direction)
    {
        lookX = direction.x * sensitivity;
        lookY = direction.y * sensitivity;

        rotX -= lookY;
        rotX = Mathf.Clamp(rotX, minRotX, maxRotX);
        rotY += lookX;

        transform.localRotation = Quaternion.Euler(rotX, rotY, rotZ);
        player.transform.Rotate(Vector3.up * lookX);
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
