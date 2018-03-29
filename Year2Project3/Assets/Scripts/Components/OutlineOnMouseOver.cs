using UnityEngine;
using cakeslice;

public class OutlineOnMouseOver : MonoBehaviour 
{

    private Outline[] outlineComponents;
    public bool canShowOutline = true;

    private void Awake()
    {
        outlineComponents = GetComponentsInChildren<Outline>();
    }

    private void Start()
    {
        for (int i = 0; i < outlineComponents.Length; i++)
        {
            outlineComponents[i].color = 1;
        }
    }

    private void OnMouseEnter()
    {
        if (!canShowOutline)
        {
            return;
        }

        for (int i = 0; i < outlineComponents.Length; i++)
        {
            outlineComponents[i].color = 0;
        }
    }

    public void OnMouseExit()
    {
        for (int i = 0; i < outlineComponents.Length; i++)
        {
            outlineComponents[i].color = 1;
        }
    }
}
