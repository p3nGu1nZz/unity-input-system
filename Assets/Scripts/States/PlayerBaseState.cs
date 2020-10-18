using UnityEngine;

public abstract class PlayerBaseState
{
    private GameManager gameManager;

    public abstract void EnterState(PlayerController player);

    public abstract void Update(PlayerController player);

    public abstract void FixedUpdate(PlayerController player);

    public abstract void OnCollisionEnter(PlayerController player, Collision collision);
}
