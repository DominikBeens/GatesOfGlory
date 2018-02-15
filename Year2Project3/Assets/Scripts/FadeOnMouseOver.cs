using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOnMouseOver : MonoBehaviour
{

    private Material myMat;

    private bool fadeIn;
    [Range(0, 1)]
    public float targetAlpha;
    public float fadeSpeed;

    private void Awake()
    {
        Renderer myRenderer = GetComponent<Renderer>();
        myRenderer.material = new Material(myRenderer.material);
        myMat = myRenderer.material;
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (myMat.color.a > targetAlpha)
            {
                myMat.color -= new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
            }
        }
        else
        {
            if (myMat.color.a < 1f)
            {
                myMat.color += new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
            }
        }
    }

    private void OnMouseEnter()
    {
        fadeIn = true;
    }

    private void OnMouseExit()
    {
        fadeIn = false;
    }
}
