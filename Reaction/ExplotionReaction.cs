using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplotionReaction : Reaction
{
    [SerializeField] private Rigidbody body = default;
    [SerializeField] private Transform forcePoint = default;
    [SerializeField] [Range(1, 10000)] private float velocity = default;
    [SerializeField] private float duration = 0.5f;

    public override void TriggerReaction()
    {
        Vector3 forceDir = (body.position - forcePoint.position).normalized;

        Debug.DrawLine(forcePoint.position, body.position, Color.magenta, 10);

        StartCoroutine(Move(forceDir));        
    }

    private IEnumerator Move(Vector3 velocity)
    {
        
        float duration = 0.5f;
        while (duration > 0)
        {
            body.velocity = velocity* this.velocity;            
            yield return new WaitForEndOfFrame();
            duration -= Time.deltaTime;
        }
    }

}


