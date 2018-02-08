using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBuilder : MonoBehaviour 
{

    public GameObject myBuildedObject;

    private void OnMouseDown()
    {
        CastleBuildUpgradeManager.instance.OpenUI(this);
    }
}
