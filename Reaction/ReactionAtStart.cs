using System.Collections;
using System.IO;
using UnityEngine;

public class ReactionAtStart : MonoBehaviour
{
    [SerializeField] private Reaction startReaction;
    [SerializeField] private Reaction resumeReaction = null;
    [SerializeField] private float delay = 0;
    void Start()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(delay);
        
        
        string destination = Application.persistentDataPath + "/save.dat";

        if (File.Exists(destination))
        {
            if(resumeReaction)
            {
                resumeReaction.TriggerReaction();
            }
            else
            {
                if(startReaction) startReaction.TriggerReaction();
            }

        }
        else
        {
            if(startReaction) startReaction.TriggerReaction();
        }
    }
}
