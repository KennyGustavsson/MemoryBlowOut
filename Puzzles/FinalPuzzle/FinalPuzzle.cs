using UnityEngine;

[RequireComponent(typeof(FinalPuzzleController))]
public class FinalPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject[] answerSlots = default;
    private FinalPuzzleSlotBase[] answers;
    
    private void Awake()
    {
        answers = new FinalPuzzleSlotBase[answerSlots.Length];
        for (int i = 0; i < answerSlots.Length; i++)
        {
            answers[i] = answerSlots[i].GetComponent<FinalPuzzleSlotBase>();
        }
    }

    public bool CheckIfAnswersIsFilled()
    {
        int isFilled = 0;
        
        foreach (var answer in answers)
        {
            if (answer.puzzleObj != null)
                isFilled += 1;
        }

        return isFilled == answers.Length;
    }
    
    public bool CheckAnswers()
    {
        foreach (FinalPuzzleSlotBase slot in answers)
        {
            if (!slot.CheckAnswer())
            {
                Lose();
                return false;
            }
        }
        
        Win();
        return true;
    }

    private void Win()
    {
    }

    private void Lose()
    {
    }
}
