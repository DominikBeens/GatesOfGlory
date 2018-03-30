using UnityEngine;

public class ScaleWithFOV : MonoBehaviour 
{

    private Camera mainCam;

    public float scaleDivider = 30f;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        transform.localScale = new Vector3(mainCam.fieldOfView / scaleDivider, mainCam.fieldOfView / scaleDivider, 1);
    }
}
