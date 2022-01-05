using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Manager that handles a lot of stuff that needs to communicate with each other.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// A static reference to itself.
    /// </summary>
    public static GameManager Instance;
    
    /// <summary>
    /// The player object where the hit box and logic is.
    /// </summary>
    [SerializeField] public GameObject playerObj  = null;
    
    /// <summary>
    /// The visuals for the player model and some other stuff.
    /// </summary>
    [SerializeField] private GameObject playerVisuals  = null;
    
    /// <summary>
    /// The input controller that handles all the input.
    /// </summary>
    public InputController inputController;

    /// <summary>
    /// The time in minutes that the game will last.
    /// </summary>
    [Header("Timer options")]
    [SerializeField] private int minutes = 10;
    
    /// <summary>
    /// The time in seconds that the game will last.
    /// </summary>
    [SerializeField] private int seconds = 0;
    
    /// <summary>
    /// The state machine that handles player behaviour and movement.
    /// </summary>
    public Player playerStateMachine;
    
    /// <summary>
    /// The timer of the game that keeps track of how much time that is left.
    /// </summary>
    public Timer timer;

    /// <summary>
    /// Holds a reference to the mouse cursor.
    /// </summary>
    public MouseCursor MouseCursor { set; private get;}

    /// <summary>
    /// Keeps track off if the game is paused or not.
    /// </summary>
    [NonSerialized] public bool isPaused = false;
    
    /// <summary>
    /// The clues the player has gathered.
    /// </summary>
    private bool[] clues = new bool[4];

    
    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;

        inputController = new InputController();

        playerStateMachine = new Player(playerObj);
        
        timer = gameObject.AddComponent<Timer>();
        timer.minutes = minutes;
        timer.seconds = seconds;
    }

    /// <summary>
    /// Disables or enables the input that is handled in the <see cref="InputController"/>.
    /// </summary>
    /// <param name="activeState"></param> If true Allow inputs, if false do not allow input.
    public void SetAllowInputs(bool activeState)
    {
        inputController.allowInput = activeState;
    }

    /// <summary>
    /// Change the hover mode of the mouse in the <see cref="MouseCursor"/> script.
    /// </summary>
    /// <param name="hover"></param>
    public void ChangeHover(MouseCursor.HoverState hover)
    {
        if (!MouseCursor)
        {
            return;
        }
        MouseCursor.SetHover(hover);
    }

    private void OnEnable()
    {
        EventManager.onPause += OnPause;
    }

    private void OnDisable()
    {
        EventManager.onPause -= OnPause;
    }

    /// <summary>
    /// Sets if the player is shown or not
    /// </summary>
    /// <param name="playerShowState"></param> if true the player is visible, if false the player is invisible.
    public void ToggleShowPlayer(bool playerShowState)
    {
        if (playerVisuals)
        {
            playerVisuals.SetActive(playerShowState);
        }
    }

    /// <summary>
    /// Gets the <see cref="playerObj"/> transform.
    /// </summary>
    /// <returns></returns>
    public Transform GetPlayerTransform()
    {
        return playerObj.transform;
    }

    private void Update()
    {
        inputController.Update();
        playerStateMachine.Execute();
    }
    
    /// <summary>
    /// Receives the pause event.
    /// </summary>
    /// <param name="paused"></param>
    private void OnPause(bool paused)
    {
        isPaused = paused;
    }
    
    
    private void OnDestroy()
    {
        playerStateMachine.Dispose();
        Instance = null;
    }
}

