using UnityEngine;

[RequireComponent(typeof (Animator), typeof(SphereCollider))]
public class BoopOnClick : MonoBehaviour 
{

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        try
        {
            anim.Play("Boop");
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
