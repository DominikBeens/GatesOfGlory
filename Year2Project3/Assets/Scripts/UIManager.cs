using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{

    public static UIManager instance;

    private static GameObject openedCastleUI;

    public GameObject goldPopup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void OpenCastleUI(GameObject builder)
    {
        // Close opened UI.
        if (openedCastleUI != null)
        {
            openedCastleUI.GetComponent<Animator>().SetTrigger("CloseUI");
        }

        openedCastleUI = builder;
    }

    public void CloseCastleUI(GameObject builder)
    {
        if (openedCastleUI == builder)
        {
            openedCastleUI = null;
        }
    }
}
