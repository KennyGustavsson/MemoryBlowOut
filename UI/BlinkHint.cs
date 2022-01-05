using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkHint : MonoBehaviour
{

    private ButtonFlashAnimator buttonFlash = null;

    private void Awake()
    {
        buttonFlash = GetComponent<ButtonFlashAnimator>();
    }

    private void OnEnable()
    {
        EventManager.onUnlockHint += buttonFlash.StartAnimation;
    }

    private void OnDisable()
    {
        EventManager.onUnlockHint -= buttonFlash.StartAnimation;
    }



}
