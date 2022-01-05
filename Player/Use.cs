using UnityEngine;
using UnityEngine.AI;
using Freya;

/// <summary>
/// The use state of the player state machine. This state is called when you use an item on a object in the game.
/// </summary>
class Use : FiniteStateMachine<Player>.State
{

    IItemReceiver receiver;
    InventoryItem itemToUse;

    private Transform playerTransform;
    private Transform interactableTransform = null;

    private Vector3 waypointGlobalPos = Vector3.zero;

    public void Init(IItemReceiver receiver, InventoryItem item)
    {
        itemToUse = item;
        this.receiver = receiver;

        playerTransform = stateMachine.player.transform;

        var baseInteractable = (receiver as InteractableBase);
        interactableTransform = baseInteractable.transform;
        waypointGlobalPos = baseInteractable.waypointPosition + interactableTransform.position;
    }

    public Use(Player stateMachine) : base(stateMachine)
    {
    }

    

    public override void End()
    {
    }

    public override void Execute()
    {
        if (NavMesh.SamplePosition(waypointGlobalPos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas)){

            stateMachine.animator.SetFloat("WalkBlend", stateMachine.agent.velocity.magnitude / stateMachine.agent.speed);
            
            if (Vector3.Distance(playerTransform.position, hit.position) < 0.5f){

                stateMachine.animator.SetFloat("WalkBlend", 0);
                stateMachine.agent.ResetPath();

                Vector3 targetDir = (interactableTransform.position - playerTransform.position);
                Vector2 horizontalDir = targetDir.xz().normalized;
                Vector2 horizontalForward = playerTransform.forward.xz();

                float turnDir = Mathfs.Determinant(horizontalDir, horizontalForward);
                if (turnDir == 0)
                    turnDir = 1;
                else
                    turnDir = turnDir / turnDir.Abs();

                float turn = stateMachine.agent.angularSpeed * Time.deltaTime;
                float angBetween = Mathfs.AngleBetween(horizontalDir, horizontalForward) * Mathfs.Rad2Deg;
                if (angBetween < turn)
                    turn = angBetween;

                if (angBetween > 1.0f)
                {
                    var quat = Quaternion.AngleAxis(turnDir * turn, Vector3.up);
                    playerTransform.rotation = quat * playerTransform.rotation;
                    return;
                }

                receiver.ReceiveItem(itemToUse);
                stateMachine.ChangeState(typeof(Idle));
            }
        }
    }

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