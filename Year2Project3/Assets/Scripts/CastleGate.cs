using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CastleGate : MonoBehaviour
{
    public Gate myDefensePoint;
    public Animator gateAnim;
    public Animator gateButtonAnim;
    public bool locked;
    public bool isOpen;

    public static void HealGate(int heal){
    }

    public void ToggleGate()
    {
        gateButtonAnim.SetTrigger("Pressed");

        if (gateAnim.GetCurrentAnimatorStateInfo(0).IsName("GateOpen"))
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

        float currentAnimationProgress = gateAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (currentAnimationProgress < 1)
        {
            gateAnim.Play("GateOpen", 0, (1 - currentAnimationProgress));
        }
        else
        {
            gateAnim.Play("GateOpen");
        }
    }

    public void CloseGate()
    {
        if (locked)
        {
            return;
        }

        float currentAnimationProgress = gateAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (currentAnimationProgress < 1)
        {
            gateAnim.Play("GateClose", 0, (1- currentAnimationProgress));
        }
        else
        {
            gateAnim.Play("GateClose");
        }
    }

    public void AnimationEventGate()
    {
        isOpen = !isOpen;
        myDefensePoint.ChangeGateOpen();
    }
}
