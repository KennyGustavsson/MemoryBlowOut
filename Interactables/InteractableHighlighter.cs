using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
public class InteractableHighlighter : MonoBehaviour
{
    private static float maxDistance = 4f;
    private static float minDistance = 0.5f;
    private static float farValue = 0f;
    private static float nearValue = 2f;


    private Material highlightMat = null;
    private int outlineThicknessID = 0;
    private MaterialPropertyBlock propertyBlock = null;

    private int highlightMatIndex = 0;

    private Transform player = null;
    new private MeshRenderer renderer;

#if UNITY_EDITOR
    private void OnEnable()
    {
        renderer = GetComponent<MeshRenderer>();
        highlightMatIndex = renderer.sharedMaterials.Length - 1;

        propertyBlock = new MaterialPropertyBlock();

        highlightMat = renderer.sharedMaterials[highlightMatIndex];
        renderer.GetPropertyBlock(propertyBlock, highlightMatIndex);
        outlineThicknessID = Shader.PropertyToID("_OutlineWidth");
    }
#endif

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        highlightMatIndex = renderer.sharedMaterials.Length - 1;


        propertyBlock = new MaterialPropertyBlock();

        highlightMat = renderer.sharedMaterials[highlightMatIndex];
        renderer.GetPropertyBlock(propertyBlock, highlightMatIndex);
        outlineThicknessID = Shader.PropertyToID("_OutlineWidth");
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (!Application.IsPlaying(gameObject))
            return;
#endif
        player = GameManager.Instance.playerStateMachine.player.transform;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.IsPlaying(gameObject))
        {
            return;
        }
#endif
        float thickness = Freya.Mathfs.Remap(
        maxDistance, minDistance, farValue, nearValue,
        Vector3.Distance(player.position, transform.position));
        thickness = Freya.Mathfs.Clamp(thickness, farValue, nearValue);

        propertyBlock.SetFloat(outlineThicknessID, thickness);
        renderer.SetPropertyBlock(propertyBlock, highlightMatIndex);
    }
}


