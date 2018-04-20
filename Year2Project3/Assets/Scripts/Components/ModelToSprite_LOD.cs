using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelToSprite_LOD : MonoBehaviour
{

    private Camera mainCam;

    private bool playerHoveredDetails;

    public float swapToSpriteFOV;
    [Space(10)]
    public GameObject details;
    public GameObject detailSprite;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!playerHoveredDetails)
        {
            if (mainCam.fieldOfView > swapToSpriteFOV)
            {
                DetailsOff();
            }
            else
            {
                DetailsOn();
            }
        }
        else
        {
            DetailsOn();
        }
    }

    private void DetailsOn()
    {
        details.SetActive(true);
        detailSprite.SetActive(false);
    }

    private void DetailsOff()
    {
        details.SetActive(false);
        detailSprite.SetActive(true);
    }

    public void TogglePlayerMouseHover(bool mouseEnter)
    {
        playerHoveredDetails = mouseEnter ? true : false;
    }
}
