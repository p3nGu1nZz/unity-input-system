using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Editor
    [SerializeField] private float moveForce = 1;
    [SerializeField] private float jumpForce = 1;

    // Variables
    private PlayerBaseState currentState;
    private InputSystemControls controls;
    private Rigidbody rb;
    private Vector3 moveVector;
    private Vector3 rotateVector;

    public InputSystemControls Controls
    {
        get { return controls; }
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
    }

    private void Start()
    {
        TransitionToState(IdleState);
    }

    private void Update()
    {
        currentState.Update(this);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
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
        moveVector = new Vector3(0f, 0f, direction.y);
        moveVector *= moveForce;
        transform.LookAt(transform.position + Vector3.forward);
        rb.AddForce(moveVector, ForceMode.Force);
    }
}
