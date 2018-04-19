using UnityEngine;

public class CursorManager : MonoBehaviour
{

    public static CursorManager instance;

    private Vector3 mousePos;
    private Camera mainCam;

    public bool cursorVisible;
    public bool CursorVisibilityStandard { get; private set; }

    public GameObject cursorObject;
    private Transform cursorObjectPos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        mainCam = Camera.main;

        if (cursorObject != null)
        {
            cursorObjectPos = Instantiate(cursorObject).transform;
        }

        Cursor.visible = cursorVisible;
        CursorVisibilityStandard = cursorVisible;
    }

    private void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - mainCam.transform.position.z;
        mousePos = mainCam.ScreenToWorldPoint(mousePos);

        if (cursorObject != null)
        {
            cursorObjectPos.position = mousePos;
        }

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 50))
        //    {
        //        print(hit.transform.gameObject.layer);
        //    }
        //}
    }

    public void ToggleCursorObject()
    {
        cursorObjectPos.gameObject.SetActive(!cursorObjectPos.gameObject.activeSelf);
    }
}
