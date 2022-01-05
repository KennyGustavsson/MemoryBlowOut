using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This is not used and should be removed.
/// </summary>
public class PlayerMovement
{
    private NavMeshAgent agent;

    public PlayerMovement(NavMeshAgent agent)
    {
	    this.agent = agent;
    }

    public void MovePosition(Vector3 position)
    {
	    agent.SetDestination(position);
    }
}
