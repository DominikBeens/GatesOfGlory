using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CastleGate : MonoBehaviour 
{

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("GateOpen"))
        {
            CloseGate();
        }
        else
        {
            OpenGate();
        }
    }

    private void OpenGate()
    {
        float currentAnimationProgress = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (currentAnimationProgress < 1)
        {
            anim.Play("GateOpen", 0, (1 - currentAnimationProgress));
        }
        else
        {
            anim.Play("GateOpen");
        }
    }

    private void CloseGate()
    {
        float currentAnimationProgress = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (currentAnimationProgress < 1)
        {
            anim.Play("GateClose", 0, (1- currentAnimationProgress));
        }
        else
        {
            anim.Play("GateClose");
        }
    }
}
