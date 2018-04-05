using System.Collections.Generic;
using UnityEngine;

public class OutlineOnMouseOver : MonoBehaviour 
{

    public Material outlineMat;
    public float outlineThickness;
    public bool canShowOutline = true;

    private List<Material> outlineMats = new List<Material>();

    private void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].sharedMaterials.Length > 1)
            {
                Material[] sharedMats = renderers[i].sharedMaterials;

                for (int ii = 0; ii < sharedMats.Length; ii++)
                {
                    if (sharedMats[ii] == outlineMat)
                    {
                        sharedMats[ii] = new Material(sharedMats[ii]);
                        outlineMats.Add(sharedMats[ii]);
                    }
                }

                renderers[i].sharedMaterials = sharedMats;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!canShowOutline)
        {
            return;
        }

        for (int i = 0; i < outlineMats.Count; i++)
        {
            outlineMats[i].SetFloat("_Thickness", outlineThickness);
        }
    }

    public void OnMouseExit()
    {
        for (int i = 0; i < outlineMats.Count; i++)
        {
            outlineMats[i].SetFloat("_Thickness", 0);
        }
    }
}
