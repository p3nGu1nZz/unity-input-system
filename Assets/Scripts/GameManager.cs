using UnityEngine;
using UnityEngine.Events;

/**
 * The root manager which controls the game. This interacts with the player controller
 * and also manages paused events.
 */
public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    private static PlayerController player;
    private GameEvent pauseGameEvent;
    private UnityAction gamePauseListener;
    private GUIStyle pausedStyle;
    private string pausedText = "PAUSED";
    private static bool isGamePaused = false;

    /**
     * helper function which returns the paused state of the game
     */
    public static bool IsGamePaused
    {
        get
        {
            return isGamePaused;
        }
    }

    /**
     * grabs a local instance of the game manager class. If it exists then do
     * not bother looking up. required for when you switch scenes
     */
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

    /**
     * initiliazes our game by creating a new event listener to detect if the 
     * game is paused. This also locates the player controller with a simple
     * find call.
     */
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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

    /**
     * the check function that determines if we should pause the game or not
     */
    private void PauseGame()
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

    /**
     * resumes the game and sets various game properties of this manager
     */
    private void GameResume()
    {
        Debug.Log("RESUME");
        isGamePaused = false;
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    /**
     * Pauses the game and unlocks the cursor so that you can click around on
     * your computer. This is a bit wonky inside the unity editor, but seems
     * to be fine in the release version
     */
    private void GamePause()
    {
        Debug.Log("PAUSE");
        isGamePaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    /**
     * unity lifecycle hook which calls the function that actually draws the 
     * gui
     */
    private void OnGUI()
    {
        if (IsGamePaused)
            DrawPauseMenu();
    }

    /**
     * draws a simple pause menu, by placing some text into the middle of the screen
     */
    private void DrawPauseMenu()
    {
        pausedStyle = GUI.skin.GetStyle("Label");
        pausedStyle.alignment = TextAnchor.UpperCenter;
        pausedStyle.fontSize = 42;
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 200, 50), pausedText, pausedStyle);
    }
}
