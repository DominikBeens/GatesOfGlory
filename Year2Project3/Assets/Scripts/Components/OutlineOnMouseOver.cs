using UnityEngine;

public class OutlineOnMouseOver : MonoBehaviour 
{

    public Material outlineMat;
    public float outlineThickness;
    public bool canShowOutline = true;

    private void OnMouseEnter()
    {
        if (!canShowOutline)
        {
            return;
        }

        outlineMat.SetFloat("_Thickness", outlineThickness);
    }

    public void OnMouseExit()
    {
        outlineMat.SetFloat("_Thickness", 0);
    }
}
