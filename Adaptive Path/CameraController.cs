using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float sensitivity = 2.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 20.0f;
    public float rotateSpeed = 10.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    [Header("Want To rotate Camera Automatically around the Target? Then Check This!")]
    public bool autoRotate = false;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (autoRotate)
        {
            InvokeRepeating("RotateCamera", 0.0f, 0.01f);
        }
    }

    void Update()
    {
        if (!autoRotate)
        {
            x += Input.GetAxis("Mouse X") * sensitivity;
            y -= Input.GetAxis("Mouse Y") * sensitivity;
            y = Mathf.Clamp(y, -90.0f, 90.0f);
        }

        distance -= Input.GetAxis("Mouse ScrollWheel") * 5;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    void RotateCamera()
    {
        x += rotateSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}
