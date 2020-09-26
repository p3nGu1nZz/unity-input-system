using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Editor
    [SerializeField] private float moveForce = 10;
    [SerializeField] private float jumpVelocity = 500;

    // Variables
    private PlayerBaseState currentState;
    private InputSystemControls controls;
    private Rigidbody rb;
    private Vector3 moveVector;

    // Getters
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

    // States
    public readonly PlayerIdleState IdleState = new PlayerIdleState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerWalkingState WalkingState = new PlayerWalkingState();

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new InputSystemControls();

        //rb.freezeRotation = true;
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
        Debug.Log("JUMP!");
        rb.AddForce(transform.up * jumpVelocity, ForceMode.Force);
    }

    public void Move(Vector3 direction)
    {
        moveVector = new Vector3(direction.x * 10f, 0f, direction.y * 10f);
        moveVector *= moveForce;
        transform.LookAt(transform.position + moveVector);
        rb.AddForce(moveVector, ForceMode.Force);
    }
}
