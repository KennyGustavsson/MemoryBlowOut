using UnityEngine;
using UnityEngine.AI;
using Freya;

/// <summary>
/// The Interact state of the player state machine.
/// </summary>
class Interact : FiniteStateMachine<Player>.State
{
    //The interactable to react with
    private IInteractable interaction;

    //The players transform 
    private Transform playerTransform;
    
    //The interactables transform
    private Transform interactableTransform = null;
    
    //The waypoint position to walk to for the interactable.
    private Vector3 waypointGlobalPos = Vector3.zero;
    
    //The waypoint rotation to rotate to for the interactable.
    private Vector3 waypointRotation;
    //If the interactable should use custom settings for the rotation of the waypoint.
    private bool useCustomRotation;

    //The constructor of the interact state
    public Interact(Player stateMachine) : base(stateMachine)
    {
    }

    
    /// <summary>
    ///Unpacks the interactable and saves all of the important information.
    /// </summary>
    /// <param name="interactable"></param> the interactable
    public void Init(IInteractable interactable)
    {
        interaction = interactable;
        playerTransform = stateMachine.player.transform;

        var baseInteractable = (interactable as InteractableBase);
        
        interactableTransform = baseInteractable.transform;
        waypointGlobalPos = baseInteractable.waypointPosition + interactableTransform.position;
        waypointRotation = baseInteractable.waypointRotation;
        useCustomRotation = baseInteractable.useCustomRotation;
    }

    public override void End()
    {
    }

    /// <summary>
    /// Gets called every frame from the state machine and checks if the player has arrived  in the correct location and
    /// is rotated in the right rotation to pick up the item. 
    /// </summary>
    public override void Execute()
    {
        if (NavMesh.SamplePosition(waypointGlobalPos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            stateMachine.animator.SetFloat("WalkBlend", stateMachine.agent.velocity.magnitude / stateMachine.agent.speed);
            
            if (Vector3.Distance(playerTransform.position, hit.position) < 0.5f)
            {
                stateMachine.animator.SetBool("Walk", false);
                stateMachine.agent.ResetPath();

                Vector3 targetDir = (interactableTransform.position - playerTransform.position);
                Vector2 horizontalDir = targetDir.xz().normalized;
                Vector2 horizontalForward = playerTransform.forward.xz();

                if (useCustomRotation)
                {
                    Vector2 dirTarget = new Vector2(Mathf.Sin(waypointRotation.y * Mathf.Deg2Rad),
                        Mathf.Cos(waypointRotation.y * Mathf.Deg2Rad));

                    horizontalDir = dirTarget;
                }

                float turnDir = Mathfs.Determinant(horizontalDir, horizontalForward);
                if (turnDir == 0)
                    turnDir = 1;
                else
                    turnDir = turnDir / turnDir.Abs();                
                
                float turn = stateMachine.agent.angularSpeed*Time.deltaTime;
                float angBetween = Mathfs.AngleBetween(horizontalDir, horizontalForward) * Mathfs.Rad2Deg;
                if (angBetween < turn)
                    turn = angBetween;
                                
                if (angBetween > 1.0f)                    
                {
                    var quat = Quaternion.AngleAxis(turnDir * turn, Vector3.up);
                    playerTransform.rotation = quat * playerTransform.rotation;
                    return;
                }
                
                interaction.OnInteract();
                stateMachine.animator.Play("Grab");
                stateMachine.ChangeState(typeof(Idle));
            }

        }
    }

    /// <summary>
    /// Called in the beginning of the state and tells the navmesh agent to walk to the interactable. 
    /// </summary>
    public override void Prepare()
    {
        if (NavMesh.SamplePosition(waypointGlobalPos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            stateMachine.agent.SetDestination(hit.position);
            stateMachine.animator.SetFloat("WalkBlend", stateMachine.agent.velocity.magnitude / stateMachine.agent.speed);
        }
        else
        {
            stateMachine.ChangeState(typeof(Idle));
        }
    }
}
