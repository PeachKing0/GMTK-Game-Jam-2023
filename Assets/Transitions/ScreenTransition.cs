using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    public static ScreenTransition Instance;
    public Animator transition;
    public Animator otherAnim;
    public float waitTime = 1f;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayAnimation(string anim)
    {
        transition.SetTrigger(anim);
    }

    public void PlayOtherAnim(string anim)
    {
        otherAnim.SetTrigger(anim);
    }
}
