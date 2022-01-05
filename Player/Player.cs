using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The player state machine that handles all player movement, interaction and animation.
/// </summary>
public class Player : FiniteStateMachine<Player>
{
    /// <summary>
    /// The gameObject of the player. The one that has the collider and logic on it.
    /// </summary>
    public GameObject player;
    
    /// <summary>
    /// The navmesh agent of the player,
    /// </summary>
    public NavMeshAgent agent;
    
    /// <summary>
    /// The animator of the player.
    /// </summary>
    public Animator animator;
    
    /// <summary>
    /// The Constructor of the player state machine, it registers events and saves references to <see cref="player"/>,
    /// <see cref="animator"/> and <see cref="agent"/>.
    /// </summary>
    /// <param name="playerObject"></param> The GameObject of the player.
    public Player(GameObject playerObject) : base(typeof(Idle))
    {
        EventManager.onMouseClickObject += Interact;
        EventManager.onMouseClickWalkable += Move;
        EventManager.onUseItem += Use;

        animator = playerObject.GetComponent<Animator>();
        player = playerObject;
        agent = player.GetComponent<NavMeshAgent>();
        if (!agent)
        {
            Debug.LogError("No NavAgent at player object");
        }
    }

    private void Use(IItemReceiver receiver, InventoryItem item)
    {
        (GetState(typeof(Use)) as Use).Init(receiver,item);
        ChangeState(typeof(Use));
    }

    private void Move(Vector3 pos)
    {
        (GetState(typeof(Move)) as Move).Init(pos);
        ChangeState(typeof(Move));
    }

    private void Interact(IInteractable interactable)
    {
        
        (GetState(typeof(Interact)) as Interact).Init(interactable);

        ChangeState(typeof(Interact));
    }



    /// <summary>
    /// Called to unsubscribe all events from the state machine. If this is not done on scene change the events, that are
    /// persistent between scenes, will keep this state machine alive and will cause problems if you restart the game
    /// without quiting the application.
    /// </summary>
    public void Dispose()
    {
        EventManager.onMouseClickObject -= Interact;
        EventManager.onMouseClickWalkable -= Move;
        EventManager.onUseItem -= Use;

    }

}
