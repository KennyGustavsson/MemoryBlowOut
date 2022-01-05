using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ToggleNavMeshAgentReaction : Reaction
{

    private NavMeshAgent agent = null;
    [SerializeField] private bool toggle = true;

    public override void TriggerReaction()
    {
        agent = GameManager.Instance.playerObj.GetComponent<NavMeshAgent>();
        agent.enabled = toggle;
    }
}
