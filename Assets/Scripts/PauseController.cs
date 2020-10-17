using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeController : MonoBehaviour
{
    private static bool gameTimeIsPaused = false;

    public void Update(PlayerController player)
    {
        if (player.Controls.Player.Pause.triggered)
        {
            Debug.Log("Pause Triggered!!!");

            if (gameTimeIsPaused)
            {
                this.Unpause();
            }
            else
            {
                this.Pause();
            }
        }
    }

    private void Start()
    {
    }

    private void Pause()
    {
        Debug.Log("Pause");
        gameTimeIsPaused = true;
    }

    /**
     * Unpauses the GameTime of the scene and dispatches an event for other 
     * callbacks to listen to.
     */
    private void Unpause()
    {
        Debug.Log("Unpause");
        gameTimeIsPaused = false; // starts the game
    }

    public bool IsPaused()
    {
        return gameTimeIsPaused;
    }
}
