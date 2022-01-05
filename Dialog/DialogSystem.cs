using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    /// <summary>
    /// The Prefab for the Dialog Box, there should be a <see cref="dialogBox"/> script on the root object of the
    /// prefab.
    /// </summary>
    [SerializeField] private GameObject dialogPrefab = default;

    private DialogBox dialogBox;
    private RectTransform dialogBoxRect;
    
    private bool dialogRunning;

    private Queue<DialogGroup> dialogQueue = new Queue<DialogGroup>();

    private CharacterTag currentDialogCharacter;
    private DialogGroup currentDialogGroup;
    private Dialog currentDialog;
    
    private GameObject dialogObject;
    private Canvas dialogCanvas;
    private WorldToOverlayPlacement dialogPlacement;
    
    private float dialogTime;
    
    private void Start()
    {
        CreateDialogCanvas();
        dialogBoxRect = Instantiate(dialogPrefab, dialogObject.transform).GetComponent<RectTransform>();
        dialogBoxRect.gameObject.SetActive(false);
        dialogBox = dialogBoxRect.GetComponent<DialogBox>();
    }

    private void OnEnable()
    {
        EventManager.onDisplayDialog += StartDialog;
        EventManager.onStopAllDialogs += StopAllDialogs;
    }

    private void OnDisable()
    {
        EventManager.onDisplayDialog -= StartDialog;
        EventManager.onStopAllDialogs -= StopAllDialogs;
    }

    /// <summary>
    /// Creates and sets up the canvas for the <see cref="DialogSystem"/>
    /// </summary>
    private void CreateDialogCanvas()
    {
        dialogObject = new GameObject();
        dialogObject.name = "dialogCanvas";
        dialogObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = dialogObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        dialogObject.AddComponent<GraphicRaycaster>();
        
        dialogPlacement = dialogObject.AddComponent<WorldToOverlayPlacement>();
    }

    private void Update()
    {
        if(!dialogRunning) return;
            UpdateDialogPosition();
    }

    /// <summary>
    /// Updates the position of <see cref="dialogObject"/>, which is an instantiation of <see cref="dialogPrefab"/>, so
    /// that it is placed above the talking npc.
    /// </summary>
    private void UpdateDialogPosition()
    {
        float offsetX = currentDialogCharacter.dialogOffset.x;
        float offsetY = currentDialogCharacter.dialogOffset.y;

        dialogBoxRect.position = dialogPlacement.WorldToOverlayPoint(currentDialogCharacter.transform.position,
            dialogBoxRect, offsetX, offsetY, dialogCanvas);

    }

    /// <summary>
    /// Iterates trough all the <see cref="Dialog"/>s in <see cref="dialogToPlay"/> and shows them one by one for the
    /// right amount of time.
    /// </summary>
    /// <param name="dialogToPlay"></param> the <see cref="DialogGroup"/> to play
    /// <returns></returns>
    private IEnumerator DialogSequence(DialogGroup dialogToPlay)
    {
        dialogRunning = true;
        dialogBox.gameObject.SetActive(true);

        currentDialogGroup = dialogToPlay;

        bool safeToRun = true;

            if (!currentDialogGroup)
            {
                Debug.LogError("dialog group: " + currentDialogGroup.name +  "  is null");
                safeToRun = false;
            }

            if (currentDialogGroup.dialogs == null)
            {
                Debug.LogError("dialogs in " + currentDialogGroup.name +  "  is null?");
                safeToRun = false;
            }

            if (safeToRun)
            {
                foreach (Dialog dialog in currentDialogGroup.dialogs)
                {
                    if (dialog == null)
                    {
                        Debug.LogError($"Dialogue is not assign in inspector in group {currentDialogGroup}.");
                        continue;
                    }
                
                    int characterID = (int) dialog.dialogCharacter;

                    currentDialogCharacter = CharacterTag.characters[characterID];
                    currentDialog = dialog;
                    

                    DisplayDialog(dialog);
                    yield return new WaitForSeconds(dialog.dialogTime);
                }
            }

        dialogBox.gameObject.SetActive(false);
        dialogRunning = false;
    }

    /// <summary>
    /// Function called to display a <see cref="Dialog"/>.
    /// </summary>
    /// <param name="dialog"></param> The dialog to display.
    private void DisplayDialog(Dialog dialog)
    {
        dialogBox.gameObject.SetActive(true);
                
        dialogBox.dialogBoxText.color = currentDialogCharacter.dialogColor;
        dialogBox.dialogBoxText.text = dialog.dialogText;
    }
    
    /// <summary>
    /// Function called to play a sequence of dialogs in a dialog group.
    /// </summary>
    /// <param name="dialogToPlay"></param> The dialog Group to play.
    private void StartDialog(DialogGroup dialogToPlay)
    {
        if (!dialogRunning)
        {
            StartCoroutine(DialogSequence(dialogToPlay));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(DialogSequence(dialogToPlay));
        }
    }

    /// <summary>
    /// Stops all running dialog.
    /// </summary>
    private void StopAllDialogs()
    {
        dialogQueue.Clear();
        StopAllCoroutines();
        dialogBox.gameObject.SetActive(false);
        dialogRunning = false;

    }
}
