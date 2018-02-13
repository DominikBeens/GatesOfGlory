using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CastleGate : MonoBehaviour 
{
    public Gate myDefensePoint;
    private Animator anim;
    public bool locked;
    public bool isOpen;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ToggleGate()
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

    public void OpenGate()
    {
        if (locked)
        {
            return;
        }

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

    public void CloseGate()
    {
        if (locked)
        {
            return;
        }

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

    public void AnimationEventGate()
    {
        isOpen = !isOpen;
        myDefensePoint.ChangeGateOpen();
    }
}
