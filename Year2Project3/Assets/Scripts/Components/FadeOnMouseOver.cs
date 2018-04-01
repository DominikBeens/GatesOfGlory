using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOnMouseOver : MonoBehaviour
{

    private bool canFade;

    private bool fadeIn;
    public List<GameObject> objectsToFade = new List<GameObject>();
    private List<Renderer> renderers = new List<Renderer>();
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

            renderers.Add(myRenderer);
        }
    }

    private void Update()
    {
        if (fadeIn)
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                if (renderers[i].material.color.a > targetAlpha)
                {
                    renderers[i].material.color -= new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
                }
            }
        }
        else
        {
            if (canFade)
            {
                for (int i = 0; i < renderers.Count; i++)
                {
                    if (renderers[i].material.color.a < 1f)
                    {
                        renderers[i].material.color += new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
                    }

                    if (i == renderers.Count - 1)
                    {
                        if (renderers[i].material.color.a > 0.99f)
                        {
                            canFade = false;
                        }
                    }
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Playing)
        {
            fadeIn = true;
            canFade = true;
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
