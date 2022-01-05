using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool isToggled;

    [HideInInspector] public float voltage = 0;

    private bool playingAnimation;

    [SerializeField] private float toggleAnimationSpeed = 10;
    [SerializeField] private Vector3 onRotation = Vector3.right * 45f;
    [SerializeField] private Vector3 offRotation = Vector3.right * -45f;

    [SerializeField] private Transform switchPivot = default;

    [Header("Reactions")]
    [SerializeField] private Reaction toggleSwitchReaction = null;

    public bool ToggleSwitch()
    {
        playingAnimation = true;
        return isToggled = !isToggled;
    }

    private void Start()
    {
        switchPivot.localRotation = Quaternion.Euler(offRotation);
        if (!switchPivot) switchPivot = transform;
    }

    private void Update()
    {
        if (playingAnimation)
        {
            if(toggleSwitchReaction)
                toggleSwitchReaction.TriggerReaction();
            
            ToggleAnimation();
        }
    }

    private void ToggleAnimation()
    {
        Vector3 toRotation = isToggled ? onRotation : offRotation;

        if (switchPivot.localRotation.eulerAngles == toRotation)
        {
            playingAnimation = false;
        }
        
        Quaternion rotation = Quaternion.Lerp(switchPivot.localRotation,Quaternion.Euler(toRotation), 
            toggleAnimationSpeed * Time.deltaTime);

        switchPivot.localRotation = rotation;
    }
}
