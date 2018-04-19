using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PreBuiltCastleRoom : MonoBehaviour 
{

    protected Transform mainCam;

    public Transform uiParent;
    public GameObject uiPanel;
    public GameObject uiOpenButton;
    [Space(10)]
    public Animator anim;
    [Space(10)]
    public OutlineOnMouseOver outline;
    public ModelToSprite_LOD lodGroup;

    public virtual void Awake()
    {
        mainCam = Camera.main.transform;

        uiPanel.SetActive(false);
    }

    public virtual void Update()
    {
        if (uiPanel.activeInHierarchy)
        {
            uiParent.LookAt(mainCam);
        }
    }

    public void OpenUIButton()
    {
        for (int i = 0; i < UIManager.instance.prebuiltCastleRooms.Length; i++)
        {
            if (UIManager.instance.prebuiltCastleRooms[i] != this)
            {
                UIManager.instance.prebuiltCastleRooms[i].CloseUIButton();
            }
        }

        uiOpenButton.SetActive(false);
        uiPanel.SetActive(true);

        if (outline != null)
        {
            outline.canShowOutline = false;
            outline.OnMouseExit();
        }
    }

    public void CloseUIButton()
    {
        StartCoroutine(CloseUI());

        if (outline != null)
        {
            outline.canShowOutline = true;
        }

        if (lodGroup != null)
        {
            lodGroup.TogglePlayerMouseHover(false);
        }
    }

    private IEnumerator CloseUI()
    {
        anim.SetTrigger("CloseUI");

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        uiPanel.SetActive(false);
        uiOpenButton.SetActive(true);
    }
}
