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
public class LookTargetController : MonoBehaviour
{
    private PlayerController player;

    [SerializeField] private float sensitivityX = 1f;
    [SerializeField] private float sensitivityY = 1f;

    private Vector2 direction;
    private float lookX, lookY;

    /**
     * Grab an instance of the camera which should be attached to this game object
     * Then we need to lock the cursor to the center of the screeb
     */
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();

        if (player == null)
            throw new System.NullReferenceException(
                "The class 'PlayerController' is null in 'LookTargetController'.");
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
        this.MoveLookTarget(direction);
    }

    /**
     * Calculates the new direction for the player to look at. This is independent of the
     * player. We calculate this value by placing a vector position into am invisible plane 
     * directly infront of the player. We then use the input signal from  the player
     * look controller input, or mouse. This will be used to translate the position of the
     * look target.
     * 
     * <param name="direction">used internally to denote the vector look position</param>
     */
    public void MoveLookTarget(Vector3 direction)
    {
        lookX = direction.x * sensitivityX;
        lookY = direction.y * sensitivityY;

        transform.Translate(new Vector3(
            1f * lookX * Time.deltaTime,
            1f * lookY * Time.deltaTime,
            0f),
            Space.Self);
    }
}
