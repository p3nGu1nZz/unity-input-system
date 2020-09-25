using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public InputSystemControls controls;
    private Rigidbody rb;
    [SerializeField] private float velocity;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new InputSystemControls();
        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Movement.performed += _ => Move(_.ReadValue<Vector2>());
    }

    private void Update()
    {

        Keyboard keyboard = InputSystem.GetDevice<Keyboard>();
        if (keyboard.spaceKey.isPressed)
        {
            Debug.Log("CHARGING");
        }
    }

    void Move(Vector2 direction)
    {
        Debug.Log("Move Player:" + direction);
    }

    void Jump()
    {
        Debug.Log("Jump!");
        rb.AddForce(0f, 1f, 0f);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
