using System.Collections;
using UnityEngine;

public class ButtonFlashAnimator : MonoBehaviour
{

    [SerializeField] private bool useScale = true;
    [SerializeField] private AnimationCurve scaleAnimation;
    [SerializeField] private bool useRotation = true;
    [SerializeField] private AnimationCurve rotateAnimation;


    private RectTransform rect = null;

    [SerializeField] private float duration;

    private float playback = 0;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

    }



    public void StartAnimation()
    {
        if(!gameObject.activeInHierarchy)
            return;
        
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    public IEnumerator Animate()
    {
        float time = 0;
        while (time < duration)
        {
            yield return new WaitForEndOfFrame();
            if (useScale)
            {
                rect.localScale = new Vector3(
                    scaleAnimation.Evaluate(time),
                    scaleAnimation.Evaluate(time));
            }
            if (useRotation)
            {
                rect.rotation = Quaternion.Euler(0, 0, rotateAnimation.Evaluate(time));
            }
            time += Time.deltaTime;
        }
    }

}
