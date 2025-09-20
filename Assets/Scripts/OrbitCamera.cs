using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Character to look at

    [Header("Orbit Settings")]
    public float distance = 5f;       // Distance from the target
    public float xSpeed = 150f;       // Horizontal swipe sensitivity
    public float ySpeed = 100f;       // Vertical swipe sensitivity
    public float yMinLimit = -20f;    // Min vertical angle
    public float yMaxLimit = 80f;     // Max vertical angle

    [Header("Offsets")]
    public Vector3 positionOffset = Vector3.zero; // Extra world position offset
    public Vector3 rotationOffset = Vector3.zero; // Extra rotation offset (Euler)

    [Header("Camera POV")]
    public Camera cam;
    public float defaultFOV = 60f;
    public float zoomFOV = 40f;        // Zoomed-in POV
    public float zoomSpeed = 5f;       // Transition speed

    private float x = 0.0f; // Current horizontal rotation
    private float y = 0.0f; // Current vertical rotation
    private bool isZoomed = false;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("OrbitCameraMobile: No target assigned!");
            enabled = false;
            return;
        }

        if (cam == null)
            cam = GetComponent<Camera>();

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        cam.fieldOfView = defaultFOV;
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleTouchInput();

        // Clamp vertical rotation
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        // Apply orbit rotation + offset
        Quaternion rotation = Quaternion.Euler(y, x, 0) * Quaternion.Euler(rotationOffset);

        // Orbit position relative to target + offset
        Vector3 position = rotation * new Vector3(0, 0, -distance) + target.position + positionOffset;

        // Update camera transform
        transform.rotation = rotation;
        transform.position = position;

        // Smooth FOV change
        float targetFOV = isZoomed ? zoomFOV : defaultFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.deltaPosition;
                x += delta.x * xSpeed * Time.deltaTime * 0.02f;
                y -= delta.y * ySpeed * Time.deltaTime * 0.02f;
            }
        }
        else if (Input.touchCount == 2)
        {
            // 🔥 Two-finger tap = toggle POV zoom
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                isZoomed = !isZoomed;
            }
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F) angle += 360F;
        if (angle > 360F) angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
