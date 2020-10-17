using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Editor
    [SerializeField] private float moveForce = 20;
    [SerializeField] private float jumpForce = 666;

    // Variables
    private PlayerBaseState currentState;
    private InputSystemControls controls;
    private GameTimeController gameTime;
    private Rigidbody rb;
    private Vector3 moveVector;

    public InputSystemControls Controls
    {
        get { return controls; }
    }

    public GameTimeController GameTime
    {
        get { return gameTime; }
    }

    public Rigidbody Rigidbody
    {
        get { return rb; }
    }

    public PlayerBaseState CurrentState
    {
        get { return currentState; }
    }

    public readonly PlayerIdleState IdleState = new PlayerIdleState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerWalkingState WalkingState = new PlayerWalkingState();

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new InputSystemControls();
        gameTime = new GameTimeController();
    }

    private void Start()
    {
        TransitionToState(IdleState);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    private void Update()
    {
        gameTime.Update(this);
        currentState.Update(this);
    }

    public void TransitionToState(PlayerBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Acceleration);
    }

    public void Move(Vector3 direction)
    {
        // old - replace with new algo
        moveVector = new Vector3(0f, 0f, direction.y);
        moveVector *= moveForce;
        transform.LookAt(transform.position + Vector3.forward);
        rb.AddForce(moveVector, ForceMode.Force);
    }
}
