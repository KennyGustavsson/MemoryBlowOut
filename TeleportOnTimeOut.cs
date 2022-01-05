using UnityEngine;
using UnityEngine.AI;

public class TeleportOnTimeOut : MonoBehaviour
{
       [SerializeField] private Transform transformToTeleportTo  = default;
       [SerializeField] private int timeToTeleportAt = 60;
       [SerializeField] private Reaction teleportReaction = null;

       private bool wasTeleported = false;

       private void OnEnable()
       {
              EventManager.onTimeUpdate += TeleportPlayer;
       }

       private void OnDisable()
       {
              EventManager.onTimeUpdate -= TeleportPlayer;
       }

       private void TeleportPlayer(int time)
       {
              if(wasTeleported || time > timeToTeleportAt) return;
              
              var playerTransform = GameManager.Instance.GetPlayerTransform();
              var playerAgent = playerTransform.GetComponent<NavMeshAgent>();
              
              ToggleAgent(playerAgent);
              playerTransform.position = transformToTeleportTo.position;
              ToggleAgent(playerAgent);
              
              wasTeleported = true;
              
              if(teleportReaction)
                     teleportReaction.TriggerReaction();
       }

       private void ToggleAgent(NavMeshAgent agent)
       {
              agent.enabled = !agent.enabled;
       }
}