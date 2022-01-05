using UnityEngine;

[RequireComponent(typeof(FinalPuzzle))]
public class FinalPuzzleController : MonoBehaviour
{
    [Header("Offset")]
    [SerializeField] private Vector3 offset = new Vector3(0f,0f, 2f);

    [Header("Buttons")] 
    [SerializeField] private GameObject exitButton = null;
    [SerializeField] private GameObject confirmButton = null;
    
    [Header("Animation")]
    [SerializeField] private float slotInSpeed = 2;
    [SerializeField] private float grabSpeed = 1;

    [Header("Reactions")]
    [SerializeField] private Reaction enterReaction = null;
    [SerializeField] private Reaction exitReaction = null;
    [SerializeField] private Reaction winReaction = null;
    [SerializeField] private Reaction loseReaction = null;
    [SerializeField] private Reaction beginDragReaction = null;
    [SerializeField] private Reaction endDragReaction = null;
    [SerializeField] private Reaction allAnswerSlotsFilledReaction = null;
    [SerializeField] private Reaction allAnswerSlotsNotFilledReaction = null;

    [Header("Limitations")]
    [SerializeField] private float grabDelay = 0.5f;
    
    private GameObject currentGameObject = null;
    private FinalPuzzleSlotBase currentSlot = null;
    private GameObject currentSlotGameObject = null;

    private bool isActive = false;
    private bool hasWon = false;
    private bool allSlotsFilled;

    private GameObject interactableObject = null;
    
    private Camera mainCamera = null;
    private FinalPuzzle finalPuzzle = null;

    private float timer = 0;

    private float grabTime = 0;

    private GameObject lastHeldObject = null;

    private void Awake()
    {
        mainCamera = Camera.main;
        finalPuzzle = GetComponent<FinalPuzzle>();
    }

    private void Update()
    {
        if(!isActive || GameManager.Instance.isPaused) return;
        
        CheckHover();
        
        if(Input.GetMouseButtonDown(0))
            OnClickDown();
        
        if(Input.GetMouseButton(0))
            OnClickHold();
        
        if(Input.GetMouseButtonUp(0))
            OnClickUp();
    }

    public void CheckHover()
    {
        if (currentGameObject)
        {
            FinalPuzzleSlotBase slot = GetSlotAtMouse();
            
            if (slot && !slot.puzzleObj)
            {
                GameManager.Instance.ChangeHover(MouseCursor.HoverState.ItemPlaceable);
                return;
            }
            
            GameManager.Instance.ChangeHover(MouseCursor.HoverState.ItemGrabbed);
        }
        else
        {
            FinalPuzzleSlotBase slot = GetSlotAtMouse();
            if (slot && slot.puzzleObj)
            {
                GameManager.Instance.ChangeHover(MouseCursor.HoverState.ItemPlaceable);
                return;
            }
            
            GameManager.Instance.ChangeHover(MouseCursor.HoverState.Normal);
        }
    }

    private void OnClickDown()
    {
        GameObject clickedObject = ScreenPointRayObject();

        if (clickedObject == exitButton){
            ExitButton();
        }

        if (clickedObject == confirmButton){
            ConfirmButton();
        }
        
        if(clickedObject == null)
            return;

        currentSlotGameObject = clickedObject;
        currentSlot = currentSlotGameObject.GetComponent<FinalPuzzleSlotBase>();

        if (currentSlot == null) 
            return;

        if (!currentSlot.puzzleObj) return;
        if(Time.time - grabTime < grabDelay && lastHeldObject == currentSlot.puzzleObj) return;
        
        grabTime = Time.time + grabDelay;
        
        lastHeldObject = currentSlot.puzzleObj;
        currentGameObject = currentSlot.puzzleObj;
        
        currentGameObject.layer = 2;

        FinalPuzzleObject puzzleObject = currentGameObject.GetComponent<FinalPuzzleObject>();
        
        if (puzzleObject != null)
            puzzleObject.TriggerReaction();
        
        
        if(beginDragReaction)
            beginDragReaction.TriggerReaction();
    }

    private void OnClickHold()
    {
        if(currentGameObject == null)
            return;

        timer += Time.deltaTime;
        
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x + offset.x, Input.mousePosition.y + offset.y, offset.z);
        currentGameObject.transform.position = Vector3.Lerp(currentGameObject.transform.position, mainCamera.ScreenToWorldPoint(cursorPoint), timer * grabSpeed);
    }

    public void ActivatePuzzle(GameObject interactableObject)
    {
        if(enterReaction) enterReaction.TriggerReaction();
        this.interactableObject = interactableObject;
        this.interactableObject.layer = 2;
        isActive = true;
    }

    public void DeactivatePuzzle()
    {
        interactableObject.layer = 0;
        isActive = false;
    }
    
    private void OnClickUp()
    {
        if(currentGameObject == null)
            return;
        
        if(endDragReaction != null)
            endDragReaction.TriggerReaction();
        
        GameObject newSlot = ScreenPointRayObject();

        if (newSlot != null)
        {
            FinalPuzzleSlotBase newPuzzleSlot = newSlot.GetComponent<FinalPuzzleSlotBase>();
            
            if (newPuzzleSlot != null && !newPuzzleSlot.puzzleObj)
            {
                SetParentAndResetPos(currentGameObject.transform, newSlot.transform);
                newPuzzleSlot.puzzleObj = currentGameObject;
                currentSlot.puzzleObj = null;
                newPuzzleSlot.ItemPlaced();
                currentSlot.ItemRemoved();
            }
            
            else if (newPuzzleSlot != null)
            {
                SetParentAndResetPos(currentGameObject.transform, newSlot.transform);
                SetParentAndResetPos(newPuzzleSlot.puzzleObj.transform, currentSlot.transform);
                
                GameObject obj = newPuzzleSlot.puzzleObj;
                
                newPuzzleSlot.puzzleObj = currentGameObject;
                currentSlot.puzzleObj = obj;

            }
            else
                SetParentAndResetPos(currentGameObject.transform, currentSlotGameObject.transform);
        }
        else
            SetParentAndResetPos(currentGameObject.transform, currentSlotGameObject.transform);

        
        currentGameObject.layer = 0;
        currentGameObject = null;
        timer = 0;

        if (finalPuzzle.CheckIfAnswersIsFilled() && allAnswerSlotsFilledReaction != null)
        {
            allSlotsFilled = true;
            allAnswerSlotsFilledReaction.TriggerReaction();
        }
        else if(allSlotsFilled)
        {
            allSlotsFilled = false;
            
            if(allAnswerSlotsNotFilledReaction != null)
                allAnswerSlotsNotFilledReaction.TriggerReaction();

        }
    }

    private FinalPuzzleSlotBase GetSlotAtMouse()
    {
        GameObject newSlot = ScreenPointRayObject();
        return !newSlot ? null : newSlot.GetComponent<FinalPuzzleSlotBase>();
    }

    private void ExitButton()
    {
        if(exitButton) exitReaction.TriggerReaction();
    }

    private void ConfirmButton()
    {
        if (hasWon)
        {
            ExitButton();
            return;
        }
        
        
        if (finalPuzzle.CheckAnswers())
        {
            if(winReaction) winReaction.TriggerReaction();
            hasWon = true;
            EventManager.OnPuzzleComplete(gameObject);
        }
        else
        {
            if(winReaction) loseReaction.TriggerReaction();
        }
    }
    
    private GameObject ScreenPointRayObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit hit) ? hit.collider.gameObject : null;
    }

    private void SetParentAndResetPos(Transform transform, Transform targetTransform)
    {
        transform.parent = targetTransform;
        StartCoroutine(TransformLerp.LerpPos(transform, Vector3.zero, slotInSpeed));
    }
}
