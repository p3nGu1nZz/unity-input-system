using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static PlayerController player;
    private GameEvent pauseGameEvent;
    private UnityAction gamePauseListener;
    private static bool isGamePaused = false;
    public static bool IsGamePaused
    {
        get
        {
            return isGamePaused;
        }
    }

    private static GameManager gameManager;
    public static GameManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!gameManager)
                {
                    Debug.LogError("Unable to Locate Active 'GameManager' class");
                }
                else
                {
                    gameManager.Init();
                }
            }

            return gameManager;
        }
    }

    void Init()
    {
        gamePauseListener = new UnityAction(PauseGame);
        player = FindObjectOfType<PlayerController>();
        pauseGameEvent = EventFactory.Create(EventNames.GAME_PAUSE, gamePauseListener);
    }

    private void Awake()
    {
        gameManager = instance;
    }

    private void OnEnable()
    {
        pauseGameEvent.Start();
    }

    private void OnDisable()
    {
        pauseGameEvent.Stop();
    }

    public void Pause()
    {
        pauseGameEvent.Trigger();
    }

    void PauseGame()
    {
        if (IsGamePaused)
        {
            Time.timeScale = 1.0f;
            AudioListener.pause = false;
            isGamePaused = !isGamePaused;
            Debug.Log("Game Unpaused");
        }
        else
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            isGamePaused = !isGamePaused;
            Debug.Log("Game Paused");
        }
    }
}
