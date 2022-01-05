using System.Collections;
using UnityEngine;

public class DisableInputReaction : Reaction
{
    [SerializeField] private bool allowState = default;
    public override void TriggerReaction()
    {
        StartCoroutine(DelayedToggleInputs());
    }

    private IEnumerator DelayedToggleInputs()
    {
        yield return null;
        GameManager.Instance.SetAllowInputs(allowState);
    }
}
