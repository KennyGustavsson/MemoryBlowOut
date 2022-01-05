using UnityEngine;

/// <summary>
/// Place an UI element to a world position
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class WorldToOverlayPlacement : MonoBehaviour
{
    private RectTransform canvasRect;
    private Vector2 uiOffset;


    

    /// <summary>
    /// Initiate
    /// </summary>
    void Start()
    {
        // Get the rect transform
        this.canvasRect = GetComponent<RectTransform>();

        // Calculate the screen offset
        this.uiOffset = new Vector2(canvasRect.sizeDelta.x / 2f, canvasRect.sizeDelta.y / 2f);
    }

    /// <summary>
    /// Move the UI element to the world position
    /// </summary>
    /// <param name="worldPosition"> </param>
    /// <param name="overlayElement"></param>
    /// <param name="offsetX"></param>
    /// <param name="offsetY"></param>
    public Vector2 WorldToOverlayPoint(Vector3 worldPosition, RectTransform overlayElement, float offsetX = 0,
        float offsetY = 150, Canvas canvas = null)
    {

        float scaleFactor = 1;

        if (canvas)
        {
            scaleFactor = canvas.scaleFactor;
        }
        
        // Get the position on the canvas
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(worldPosition);

        float panelWidth = canvasRect.rect.width * canvasRect.localScale.x;
        float panelHeight = canvasRect.rect.height * canvasRect.localScale.y;

        Vector2 proportionalPosition = new Vector2(viewportPos.x * panelWidth, viewportPos.y * panelHeight);
        //new Vector2(viewportPos.x , viewportPos.y);

        Vector2 elementSize = overlayElement.rect.size * canvasRect.localScale * 0.5f;
        
        return (proportionalPosition + new Vector2(offsetX, offsetY)) / scaleFactor; 
    }
}