using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private Animator anim;

    public void playAnim(string anim)
    {
        this.anim.Play(anim);
    }
}
