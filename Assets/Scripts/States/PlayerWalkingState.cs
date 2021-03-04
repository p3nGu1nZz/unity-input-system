using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    private Vector2 direction;

    public override void EnterState(PlayerController player)
    {
        Debug.Log("Enter Walking State");
    }

    public override void FixedUpdate(PlayerController player)
    {
        direction = player.Controls.Player.Move.ReadValue<Vector2>();

        if (direction.x > 0 || direction.x < 0 || direction.y > 0 || direction.y < 0)
        {
            player.Move(direction);
        }
        else if (player.RB.velocity.magnitude == 0)
        {
            player.TransitionToState(player.IdleState);
        }
    }

    public override void Update(PlayerController player)
    {
        if (player.Controls.Player.Jump.triggered)
        {
            player.TransitionToState(player.JumpingState);
        }
    }

    public override void LateUpdate(PlayerController player)
    {
    }

    public override void OnCollisionEnter(PlayerController player, Collision collision)
    {
    }
}
