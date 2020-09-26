using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        Debug.Log("Enter Idle State");
    }

    public override void OnCollisionEnter(PlayerController player, Collision collision)
    {
    }

    public override void FixedUpdate(PlayerController player)
    {
    }

    public override void Update(PlayerController player)
    {
        if (player.Controls.Player.Move.triggered)
            player.TransitionToState(player.WalkingState);

        if (player.Controls.Player.Jump.triggered)
            player.TransitionToState(player.JumpingState);
    }
}
