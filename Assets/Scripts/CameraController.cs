using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The class that is used to controll the movement of the camera in relation to
 * the player. This is not tightly coupled to the player controller for
 * extensibility. Future work on this class will include management and
 * integration of the unity cinemachine system. This should clear up some of the 
 * bugs which manually positioning the camera creates.
 * 
 * <remarks>requires 'Camera' component to work; make sure one is included in scene</remarks>
 */
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public static CameraController cam;
    private Camera cam_;
    private PlayerController player;

    [SerializeField] private Transform CameraPosition;
    [SerializeField] private bool enablePlayerRotation;
    [SerializeField] [Tooltip("Disable for quadruped players")] private float sensitivity = 2;
    [SerializeField] private float maxRotX = 80;
    [SerializeField] private float minRotX = -80;
    [SerializeField] private float lookSpeed = 0.420f;

    private float lookX, lookY;
    private Transform lookFrom, lookTo;
    private Vector2 direction;
    private float rotX = 0.0f, rotY = 0.0f;
    private float rotZ = 0.0f;

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

    /**
     * Reads our mouse input or right controller stick to look. This happens during the
     * update because we are technically not moving a rigidody with this. This moves the 
     * head which directs the rotational angle to move towards. Also check to see if we 
     * are paused, if not then look toward the mouse movement.
     */
    private void LateUpdate()
    {
        if (GameManager.IsGamePaused) return;

        direction = player.Controls.Player.Look.ReadValue<Vector2>();
        this.Look(direction);
    }

    /**
     * Calculates the new direction for the player to look at. This is independent of 
     * the player states. We assume that we always want the player to be looking around
     * unless this state is disabled for a cut scene or something.
     * 
     * TODO boolean toggle for allowing the camera to rotate the player -- for quadped
     * 
     * <param name="direction">used internally to denote the vector look position</param>
     */
    public void Look(Vector3 direction)
    {
        lookX = direction.x * sensitivity;
        lookY = direction.y * sensitivity;

        rotX -= lookY;
        rotX = Mathf.Clamp(rotX, minRotX, maxRotX);
        rotY += lookX;

        transform.localRotation = Quaternion.Euler(rotX, rotY, rotZ);

        if(enablePlayerRotation)
            player.transform.Rotate(Vector3.up * lookX);

        // do this last
        transform.position = CameraPosition.position;
    }

    /**
     * This can be called anytime during the update method. This function
     * will wait until the end of the frame, while adding a small random 
     * increment or decrement. 
     * 
     * <param name="magnitude">the amount of force to shake the camera</param>
     * <param name="duration">the length in unscaled time to shake for</param>
     */
    public void ShakeCamera(float magnitude, float duration)
    {
        StartCoroutine(ICameraShaker(magnitude, duration));
    }

    /**
     * Runs a loop to randomly shake the camera z axis every frame for the 
     * given duration. Can be called multiple times. be careful young skywalker
     * 
     * <param name="magnitude">the amount of force used in calculation</param>
     * <param name="duration">the length in time used by the algorithm</param>
     */
    private IEnumerator ICameraShaker(float magnitude, float duration)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        for (float t = 0.0f; t <= duration; t += Time.deltaTime)
        {
            rotZ = Random.Range(-magnitude, magnitude) * (t / duration - 1.0f);
            yield return wait;
        }
        rotZ = 0.0f;
    }
}
