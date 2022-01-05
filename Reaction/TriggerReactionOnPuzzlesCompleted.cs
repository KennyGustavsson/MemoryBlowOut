using UnityEngine;

public class TriggerReactionOnPuzzlesCompleted : MonoBehaviour
{
    [Header("Puzzles")]
    [SerializeField] private SwitchBoard switchPuzzle = null;
    [SerializeField] private FinalPuzzleController dragAndDropPuzzle = null;
    [SerializeField] private PuzzleRotatable puzzleRotatable = null;

    [Header("Reaction")]
    [SerializeField] private Reaction allCompleteReaction = null;
    
    private bool hasTriggered = false;
    private bool[] allComplete = new bool[3];
    
    private void OnEnable()
    {
        EventManager.onPuzzleComplete += OnPuzzleComplete;
    }

    private void OnDisable()
    {
        EventManager.onPuzzleComplete -= OnPuzzleComplete;
    }

    private void OnPuzzleComplete(GameObject obj)
    {
        if (obj == switchPuzzle.gameObject)
        {
            allComplete[0] = true;
        }
        else if(obj == dragAndDropPuzzle.gameObject)
        {
            allComplete[1] = true;
        }
        else if(obj == puzzleRotatable.gameObject)
        {
            allComplete[2] = true;
        }

        foreach (var puzzleComplete in allComplete){
            if(!puzzleComplete) return;
        }
        
        if(allCompleteReaction)
            allCompleteReaction.TriggerReaction();
    }
}
