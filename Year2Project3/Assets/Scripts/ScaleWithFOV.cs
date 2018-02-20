using UnityEngine;

public class ScaleWithFOV : MonoBehaviour 
{

    public float scaleDivider = 30f;

    private void Update()
    {
        transform.localScale = new Vector3(Camera.main.fieldOfView / scaleDivider, Camera.main.fieldOfView / scaleDivider, 1);
    }
}
