using UnityEngine;

public class PlayerDeathReactor : MonoBehaviour
{
    [SerializeField] private Reaction deathReaction = default;
    
    private void OnEnable()
    {
        EventManager.onPlayerDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        EventManager.onPlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        
        deathReaction.TriggerReaction();
    }
}
