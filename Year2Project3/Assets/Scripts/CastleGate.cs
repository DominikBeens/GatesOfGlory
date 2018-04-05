using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CastleGate : CastleDeffensePoint
{

    private Throne throne;

    public Animator gateAnim;
    public Animator gateButtonAnim;
    public bool locked;
    public bool isOpen;

    private void Awake()
    {
        throne = FindObjectOfType<Throne>();
    }

    public void ToggleGate()
    {
        if (GameManager.instance.gameState != GameManager.GameState.Playing)
        {
            return;
        }

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
    }

    public override void TakeDamage(float damage)
    {
        if (isOpen)
        {
            return;
        }

        myStats.health.currentValue -= damage;
        healthbarFill.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);
        if (myStats.health.currentValue < 0)
        {
            myGate.OpenGate();
            isOpen = true;
            myGate.locked = true;
            foreach (Enemy e in attackingMe)
            {
                if (e != null)
                {
                    e.attackingCastle = false;
                    e.FindNewTarget();
                }
            }
        }

        throne.HPBar();
    }

    public void Heal(float healAmount)
    {
        if (myStats.health.currentValue == myStats.health.baseValue)
        {
            return;
        }

        myStats.health.currentValue += healAmount;

        Vector3 healParticlePos = new Vector3(transform.position.x + 0.5f, transform.position.y + 3, transform.position.z - 5);
        ObjectPooler.instance.GrabFromPool("heal particle", healParticlePos, Quaternion.identity);

        if (myStats.health.currentValue > myStats.health.baseValue)
        {
            myStats.health.currentValue = myStats.health.baseValue;
        }

        healthbarFill.fillAmount = myStats.health.currentValue / myStats.health.baseValue;
    }
}
