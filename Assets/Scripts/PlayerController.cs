using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputSystemControls))]
public class PlayerController : MonoBehaviour
{
    // Required 
    public InputSystemControls Controls { get; private set; }
    public Rigidbody RB { get; private set; }
    

    // TODO refactor states into there own machine / factory (decouple from player)

    // TODO implement state factory machine for this instead

    // State Management Stuff
    public PlayerBaseState CurrentState { get; private set; }
    public readonly PlayerIdleState IdleState = new PlayerIdleState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerWalkingState WalkingState = new PlayerWalkingState();

    // Editor Variables and Flags (default values)
    public Transform LookTarget;
    public float cameraSensitivity = 3f;
    public float speed = 10.0f;
    public float gravity = 10.0f;
    public float extraGravity = 5;              // >1 less floaty gravity
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 4.2f;

    // Controller Variables
    private Vector2 lookDirection;          // position in space to look at
    //private bool grounded = false;    // is player touching the ground?

    /**
     * Grab an instance of our rigidbody and controls. Also make sure that we disable
     * our gravity and freeze rotation. Unity gravity is not programmed correctly, and
     * should bethe square power of gravity, not 
     */
    public void Awake()
    {
        Controls = new InputSystemControls();       // load out control system
        RB = GetComponent<Rigidbody>();             // reference rigidbody physics
        RB.freezeRotation = true;                   // disables tumbling around
        RB.useGravity = false;                      // use our own custom gravity

        // error checking for sanity
        if (LookTarget == null)
        {
            LookTarget = transform.Find("Look Target"); // look for default if not defined
            if(LookTarget == null)                      // check if the scene has a target
                throw new System.NullReferenceException(
                    "Please set `Look Target` property of the `PlayerController`");
        }
            
    }

    private void Start()
    {
        TransitionToState(IdleState);   // default state is `Idle`
    }

    private void FixedUpdate()
    {
        if (GameManager.IsGamePaused) return;       // always check for paused state

        CurrentState.FixedUpdate(this);             // call FixedUp on states
        GameUtil.ApplyGravityToRB(RB, gravity);     // always apply gravity
    }

    private void Update()
    {
        if (Controls.Player.Pause.triggered)
        {
            EventManager.TriggerEvent(EventNames.GAME_PAUSE.ToString());
        }
        if (GameManager.IsGamePaused) return;   //always check game is paused

        CurrentState.Update(this);              // else call our state update
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

        lookDirection = this.Controls.Player.Look.ReadValue<Vector2>();
        this.MoveLookTarget(lookDirection);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CurrentState.OnCollisionEnter(this, collision);     // pipe to state

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
        velocityChange.x = 0f;
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0f;
        RB.AddForce(velocityChange, ForceMode.VelocityChange);

        // TODO make sure to apply forward Lerp force from jump -> idle | walk

        // TODO implement ground check algo for `bool grounded`

        // TODO do not strafe left and right -- use rotate around instead.

        // TODO implement the jump algo -- but in the jump state

        // TODO allow left and right lean amount while in air

        // TODO implement proper gravity algorithm

        //RB.AddForce(new Vector3(0, -gravity * RB.mass, 0)); //gravity
    }

    public void Jump()
    {
        RB.velocity = new Vector3(
            RB.velocity.x, 
            GameUtil.CalculateJumpVerticalSpeed(RB.mass, jumpHeight, gravity, extraGravity), 
            RB.velocity.z);
    }

    /**
     * Calculates the new direction for the player to look at. This is independent of the
     * player. We calculate this value by placing a vector position into am invisible plane 
     * directly infront of the player. We then use the input signal from  the player
     * look controller input, or mouse. This will be used to translate the position of the
     * look target. The senstivive and camera controls should be modified by the virtual 
     * camera in cinemachine
     * 
     * <param name="direction">used internally to denote the vector look position</param>
     */
    public void MoveLookTarget(Vector3 direction)
    {
        // TODO clamp axis values

        LookTarget.Translate(new Vector3(
            cameraSensitivity * direction.x * Time.deltaTime,
            cameraSensitivity * direction.y * Time.deltaTime,
            0f),
            Space.Self);
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
