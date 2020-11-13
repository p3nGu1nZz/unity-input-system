using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputSystemControls))]
public class PlayerController : MonoBehaviour
{
    // Editor
    //[SerializeField] private float moveForce = 64;
    [SerializeField] private float jumpForce = 640;

    // Required 
    public InputSystemControls Controls { get; private set; }
    public Rigidbody RB { get; private set; }

    // State Management Stuff
    public PlayerBaseState CurrentState { get; private set; }
    public readonly PlayerIdleState IdleState = new PlayerIdleState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerWalkingState WalkingState = new PlayerWalkingState();

    /**
     * Grab an instance of our rigidbody and controls. Also make sure that we disable
     * our gravity and freeze rotation. Unity gravity is not programmed correctly, and
     * should bethe square power of gravity, not 
     */
    public void Awake()
    {
        RB = GetComponent<Rigidbody>();
        RB.freezeRotation = true;

        // TODO turn off when we have implemented custom gravity

        RB.useGravity = true;

        Controls = new InputSystemControls();
    }

    private void Start()
    {
        TransitionToState(IdleState);
    }

    private void FixedUpdate()
    {
        CurrentState.FixedUpdate(this);
        transform.Rotate(0f, 1f * Time.deltaTime, 0.0f, Space.Self);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CurrentState.OnCollisionEnter(this, collision);

    }

    private void Update()
    {
        if (Controls.Player.Pause.triggered)
        {
            EventManager.TriggerEvent(EventNames.GAME_PAUSE.ToString());
        }

        if (!GameManager.IsGamePaused)
        {
            CurrentState.Update(this);
        }
    }

    public void TransitionToState(PlayerBaseState state)
    {
        CurrentState = state;
        CurrentState.EnterState(this);
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }

    public void Jump()
    {
        RB.AddForce(transform.up * jumpForce, ForceMode.Acceleration);
    }

    // TODO move this up to the top of the class

    public float speed = 10.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    //private bool grounded = false;

    public void Move(Vector3 direction)
    {

        // TODO please recycle your vectors.

        // calculate the velocity
        Vector3 targetVelocity = new Vector3(direction.x, 0, direction.y);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        // apply force to system
        Vector3 velocity = RB.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0f;
        RB.AddForce(velocityChange, ForceMode.VelocityChange);

        // TODO implement ground check algo for `bool grounded`

        // TODO do not strafe left and right -- use rotate around instead.

        // TODO implement the jump algo -- but in the jump state

        // TODO implement proper gravity algorithm
    }
}



/**
 * reference class from unity system internals
 */

//using UnityEngine;
//using System.Collections;

//[RequireComponent (typeof (Rigidbody))]
//[RequireComponent (typeof (CapsuleCollider))]

//public class CharacterControls : MonoBehaviour {

//	public float speed = 10.0f;
//	public float gravity = 10.0f;
//	public float maxVelocityChange = 10.0f;
//	public bool canJump = true;
//	public float jumpHeight = 2.0f;
//	private bool grounded = false;



//	void Awake () {
//	    rigidbody.freezeRotation = true;
//	    rigidbody.useGravity = false;
//	}

//	void FixedUpdate () {
//	    if (grounded) {
//	        // Calculate how fast we should be moving
//	        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
//	        targetVelocity = transform.TransformDirection(targetVelocity);
//	        targetVelocity *= speed;

//	        // Apply a force that attempts to reach our target velocity
//	        Vector3 velocity = rigidbody.velocity;
//	        Vector3 velocityChange = (targetVelocity - velocity);
//	        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
//	        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
//	        velocityChange.y = 0;
//	        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

//	        // Jump
//	        if (canJump && Input.GetButton("Jump")) {
//	            rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
//	        }
//	    }

//	    // We apply gravity manually for more tuning control
//	    rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));

//	    grounded = false;
//	}

//	void OnCollisionStay () {
//	    grounded = true;    
//	}

//	float CalculateJumpVerticalSpeed () {
//	    // From the jump height and gravity we deduce the upwards speed 
//	    // for the character to reach at the apex.
//	    return Mathf.Sqrt(2 * jumpHeight * gravity);
//	}
//}
