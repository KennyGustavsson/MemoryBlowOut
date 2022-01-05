using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The move state of the player state machine.
/// </summary>
class Move : FiniteStateMachine<Player>.State
{
    //Reference to the state machine.
    private Player stateMachine;

    //Position to walk to
    private Vector3 targetPosition;
    
    //The transform of the player character.
    private Transform playerTransform;
    
    /// <summary>
    /// sets the position the player should walk to and saves a reference to the player.
    /// </summary>
    /// <param name="targetPos"></param>
    public void Init(Vector3 targetPos)
    {
        targetPosition = targetPos;
        playerTransform = stateMachine.player.transform;
    }

    //The constructor of the state that saves a reference to the state machine.
    public Move(Player stateMachine) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void End()
    {
    }

    /// <summary>
    /// Called every frame and checks if the player has reached the target position. if it has it exits the state
    /// </summary>
    public override void Execute()
    {
        Debug.DrawLine(targetPosition, stateMachine.agent.destination);
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 10, NavMesh.AllAreas))
        {            
            stateMachine.animator.SetFloat("WalkBlend", stateMachine.agent.velocity.magnitude / stateMachine.agent.speed);
            
            if (Vector3.Distance(playerTransform.position, hit.position) < 0.5f)
            {
                stateMachine.ChangeState(typeof(Idle));
            }
        }
    }

    /// <summary>
    /// Tells the navmesh agent to walk to the target position.
    /// </summary>
    public override void Prepare()
    {
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 2, NavMesh.AllAreas))
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
