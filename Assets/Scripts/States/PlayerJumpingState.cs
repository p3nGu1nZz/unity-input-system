using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        Debug.Log("Enter Jumping State");
        player.Jump();
    }

    public override void OnCollisionEnter(PlayerController player, Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Debug.Log("Landed On Floor");
            player.TransitionToState(player.IdleState);
        }
        else if (collision.gameObject.tag == "Landable")
        {
            Debug.Log("Landed On Landable");
            player.TransitionToState(player.IdleState);
        }
    }

    public override void FixedUpdate(PlayerController player)
    {
    }

    public override void Update(PlayerController player)
    {
    }
}
