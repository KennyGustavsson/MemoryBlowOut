using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateButtonFlashReaction : Reaction
{

    ButtonFlashAnimator animationObject;

    public override void TriggerReaction()
    {
        animationObject.StartAnimation();
    }
}
