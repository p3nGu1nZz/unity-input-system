using UnityEngine;

public class GameUtil : MonoBehaviour
{
    /**
     * A general purpose function used to calaculate gravity on our Rigidbodies
     */
    public static void ApplyGravityToRB(Rigidbody RB, float gravity)
    {
        RB.AddForce(new Vector3(0f, -gravity * RB.mass, 0f));
    }

    /**
     * Calculates our vertical vector for player jumping
     */
    public static float CalculateJumpVerticalSpeed(float mass, float jumpHeight, float gravity, float extraGravity)
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(jumpHeight * gravity * extraGravity / mass);
    }
}
