using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static PlayerController player;
    private GameEvent pauseGameEvent;
    private UnityAction gamePauseListener;
    private GUIStyle pausedStyle;
    private string pausedText = "PAUSED";
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
            GameResume();
        }
        else
        {
            GamePause();
        }
    }

    void GameResume()
    {
        Debug.Log("RESUME");
        isGamePaused = false;
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void GamePause()
    {
        Debug.Log("PAUSE");
        isGamePaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    private void OnGUI()
    {
        if (IsGamePaused)
            DrawPauseMenu();
    }

    private void DrawPauseMenu()
    {
        pausedStyle = GUI.skin.GetStyle("Label");
        pausedStyle.alignment = TextAnchor.UpperCenter;
        pausedStyle.fontSize = 42;
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 200, 50), pausedText, pausedStyle);
    }
}
