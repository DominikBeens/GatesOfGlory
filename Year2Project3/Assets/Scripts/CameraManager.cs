using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour 
{

    private Camera cam;

    public Transform target;

    private Vector3 offset;

    public bool secondaryCamera;

    [Header("Speed")]
    public float smoothSpeed;
    public float moveSpeed;

    public AnimationCurve speedCurveMultiplier = new AnimationCurve(new Keyframe[3]);

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

        Keyframe[] speedKeys = speedCurveMultiplier.keys;
        speedKeys[0].time = startX - camMoveRangeX;
        speedKeys[1].time = 0;
        speedKeys[2].time = startX + camMoveRangeX;

        speedCurveMultiplier = new AnimationCurve(speedKeys);
    }

    private void Update()
    {
        if (secondaryCamera)
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            MoveCam();
        }

        transform.LookAt(target);

        ZoomCam();
    }

    private void LateUpdate()
    {
        // Lerping camera position.
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * smoothSpeed);
    }

    private void MoveCam()
    {
        // Adjusting speed according to the speed animation curve.
        float speed = speedCurveMultiplier.Evaluate(target.position.x) * moveSpeed;

        // Axis inputs.
        float x = Input.GetAxis("Horizontal") * (Time.deltaTime * speed);
        float y = Input.GetAxis("Vertical") * (Time.deltaTime * speed);

        // Move target position while clamping its X and Y.
        target.position = new Vector2(Mathf.Clamp(target.position.x + x, startX - camMoveRangeX, startX + camMoveRangeX), 
                                      Mathf.Clamp(target.position.y + y, startY - camMoveRangeYDown, startY + camMoveRangeYUp));
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
    }
}
