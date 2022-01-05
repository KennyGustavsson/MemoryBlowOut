using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpReaction : Reaction
{

    [SerializeField] private Vector3 waypointPosition = Vector3.zero;
    [SerializeField] private Vector3 waypointRotation = Vector3.zero;

    [Range(0.1f,2f)]
    [SerializeField] private float duration = default;
    [SerializeField] private Reaction nextReaction = null;

    private Transform player = null;
    


    public override void TriggerReaction()
    {
        player = GameManager.Instance.playerObj.transform;
        StartCoroutine(Jump());
    }


    public IEnumerator Jump()
    {

        Vector3 landingPos = transform.TransformPoint(waypointPosition);
        AnimationCurve posXCurve = new AnimationCurve();
        AnimationCurve posYCurve = new AnimationCurve();
        AnimationCurve posZCurve = new AnimationCurve();

        AnimationCurve rotYCurve = new AnimationCurve();

        void addPosKey(float time, Vector3 pos)
        {
            posXCurve.AddKey(time, pos.x);
            posYCurve.AddKey(time, pos.y);
            posZCurve.AddKey(time, pos.z);
        }

        addPosKey(0.0f,player.position);
        var yDiff = Mathf.Abs( landingPos.y - player.position.y);
        var midPos = (player.position + landingPos) / 2f;
        midPos.y = midPos.y+(yDiff * 0.25f);
        addPosKey(duration*0.5f, midPos);
        addPosKey(duration, landingPos);

        rotYCurve.AddKey(0,player.transform.eulerAngles.y);
        rotYCurve.AddKey(duration, waypointRotation.y);

        float timer = 0;
        while (timer <= duration)
        {
            yield return null;
            player.position = new Vector3(
                posXCurve.Evaluate(timer), 
                posYCurve.Evaluate(timer), 
                posZCurve.Evaluate(timer));
            player.rotation = Quaternion.Euler(new Vector3(0, rotYCurve.Evaluate(timer), 0));
            timer += Time.deltaTime;
        }
        nextReaction?.TriggerReaction();

    }


}
