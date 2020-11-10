using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is used to control the camera look target game object which drives
 * the cinemachine camera engine. We read the input value from the player Controls
 * which then get translated into a look direction vector. By using some simple
 * calculus to convert this cartesian vector into a spherical plane which moves
 * the look target around the plane.
 * 
 * <remarks>requires 'Camera' component to work; make sure one is included in scene</remarks>
 */
public class CameraLookTargetController : MonoBehaviour
{
    //public static CameraLookTargetController cameraTarget;
    //private Camera cam_;
    private PlayerController player;

    //[SerializeField] private Transform LookTarget;
    //[SerializeField]
    //[Tooltip("Disable transform rotation for quadruped players, and enable for FPS")]
    //private bool enablePlayerRotation = true;
    [SerializeField] private float sensitivity = 2;
    //[SerializeField] private float speed = 0.420f;
    [SerializeField] private float maxRotX = 80;
    [SerializeField] private float minRotX = -80;
    [SerializeField] private float maxRotY = 80;
    [SerializeField] private float minRotY = -80;

    private Vector2 direction;
    private float lookX, lookY;
    private float rotY = 0.0f, rotX = 0.0f, rotZ = 0.0f;

    /**
     * Grab an instance of the camera which should be attached to this game object
     * Then we need to lock the cursor to the center of the screeb
     */
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();

        if (player == null)
            throw new System.NullReferenceException(
                "The class 'PlayerController' is null in 'CameraLookTargetController'.");
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
        //this.MoveLookTarget(direction);
    }

    /**
     * Calculates the new direction for the player to look at. This is independent of 
     * the player states. We assume that we always want the player to be looking around
     * unless this state is disabled for a cut scene or something. Make sure to update
     * the camera position last to reduce jitter.
     * 
     * TODO boolean toggle for allowing the camera to rotate the player -- for quadped
     * 
     * <param name="direction">used internally to denote the vector look position</param>
     */
    public void MoveLookTarget(Vector3 direction)
    {
        lookX = direction.x * sensitivity;
        lookY = direction.y * sensitivity;

        rotX += lookX;
        rotX = Mathf.Clamp(rotX, minRotX, maxRotX);

        rotY -= lookY;
        rotY = Mathf.Clamp(rotY, minRotY, maxRotY);
        
        transform.localRotation = Quaternion.Euler(rotY, rotX, rotZ);
    }
}
