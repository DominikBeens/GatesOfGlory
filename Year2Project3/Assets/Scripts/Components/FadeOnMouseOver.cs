using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOnMouseOver : MonoBehaviour
{

    private bool fadeIn;
    public List<GameObject> objectsToFade = new List<GameObject>();
    [Range(0, 1)]
    public float targetAlpha;
    public float fadeSpeed;

    private void Start()
    {
        for (int i = 0; i < objectsToFade.Count; i++)
        {
            Renderer myRenderer = objectsToFade[i].GetComponent<Renderer>();
            myRenderer.material = new Material(myRenderer.material);

            myRenderer.material.SetFloat("_Mode", 2.0f);
            myRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            myRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            myRenderer.material.SetInt("_ZWrite", 0);
            myRenderer.material.DisableKeyword("_ALPHATEST_ON");
            myRenderer.material.EnableKeyword("_ALPHABLEND_ON");
            myRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            myRenderer.material.renderQueue = 3000;
        }
    }

    private void Update()
    {
        if (fadeIn)
        {
            for (int i = 0; i < objectsToFade.Count; i++)
            {
                if (objectsToFade[i].GetComponent<Renderer>().material.color.a > targetAlpha)
                {
                    objectsToFade[i].GetComponent<Renderer>().material.color -= new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
                }
            }
        }
        else
        {
            for (int i = 0; i < objectsToFade.Count; i++)
            {
                if (objectsToFade[i].GetComponent<Renderer>().material.color.a < 1f)
                {
                    objectsToFade[i].GetComponent<Renderer>().material.color += new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Playing)
        {
            fadeIn = true;
        }
    }

    private void OnMouseExit()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Playing)
        {
            fadeIn = false;
        }
    }
}
