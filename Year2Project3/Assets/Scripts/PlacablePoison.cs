using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacablePoison : MonoBehaviour 
{

    public GameObject poisonObject;

    private void Awake()
    {
        poisonObject.SetActive(false);
    }
}
