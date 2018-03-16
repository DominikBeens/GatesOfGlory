using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{

    public bool canMove = true;

    public GameObject ear;

    private Camera cam;

    public Transform target;

    private Vector3 offset;

    public bool secondaryCamera;

    [Header("Speed")]
    public float smoothSpeed;
    public float moveSpeed;

    [Header("Move Restrictions")]
    public float camMoveRangeX;
    public float camMoveRangeYUp;
    public float camMoveRangeYDown;
    private float startX;
    private float startY;

    [Header("Zoom")]
    public float zoomSensitivity;
    public float zoomSpeed;
    public float maxZoomIn;
    public float maxZoomOut;

    private float fov;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        startX = target.position.x;
        startY = target.position.y;
        offset = transform.position - target.position;

        fov = Camera.main.fieldOfView;
    }

    private void Update()
    {
        if (!secondaryCamera)
        {
            if (canMove)
            {
                MoveCam();
            }
        }

        transform.LookAt(target);

        if (canMove)
        {
            ZoomCam();
        }
    }

    private void LateUpdate()
    {
        // Lerping camera position.
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * smoothSpeed);

        if (secondaryCamera)
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    private void MoveCam()
    {
        // Axis inputs.
        float x = Input.GetAxis("Horizontal") * (Time.deltaTime * moveSpeed);
        float y = Input.GetAxis("Vertical") * (Time.deltaTime * moveSpeed);

        // Move target position while clamping its X and Y.
        target.position = new Vector3(Mathf.Clamp(target.position.x + x, startX - camMoveRangeX, startX + camMoveRangeX),
                                      Mathf.Clamp(target.position.y + y, startY - camMoveRangeYDown, startY + camMoveRangeYUp),
                                      target.position.z);
    }

    private void ZoomCam()
    {
        // Claming FOV.
        fov -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        fov = Mathf.Clamp(fov, maxZoomIn, maxZoomOut);

        // Changing FOV (Choppy).
        //Camera.main.fieldOfView = fov;

        // Changing FOV (Smooth).
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.deltaTime * zoomSpeed);
        ear.transform.localPosition = new Vector3(0, 0, -+(cam.fieldOfView / 90 * 21) + 21);
    }
}
