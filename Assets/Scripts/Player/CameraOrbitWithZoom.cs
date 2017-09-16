using UnityEngine;
using System.Collections;

public class CameraOrbitWithZoom : MonoBehaviour
{
    public Transform target;
    public float panSpeed = 5f;
    public float sensitivity = 1f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public float maxClamp = 90f;
    public float minClamp = 10f;

    private float distance = 0f;
    private float x = 0.0f;
    private float y = 0.0f;

    public enum MouseButton
    {
        LEFTMOUSE = 0,
        RIGHTMOUSE = 1,
        MIDDLEMOUSE = 2,
    }

    void Start()
    {
        target.transform.SetParent(null);
        distance = Vector3.Distance(target.position, transform.position);
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;
        HideCursor(true);
    }

    void Orbit()
    {
        x -= Input.GetAxis("Mouse Y") * sensitivity;
        y += Input.GetAxis("Mouse X") * sensitivity;

        if (x > maxClamp)
        {
            x = maxClamp;
        }

        if (x < minClamp)
        {
            x = minClamp;
        }
    }

    void Movement()
    {
        if (target != null)
        {
            Quaternion rotation = Quaternion.Euler(x, y, 0);
            float desiredDist = distance - Input.GetAxis("Mouse ScrollWheel");
            desiredDist = desiredDist * sensitivity;

            distance = Mathf.Clamp(desiredDist, distanceMin, distanceMax);

            Vector3 invDistanceZ = new Vector3(0, 0, -distance);
            invDistanceZ = rotation * invDistanceZ;

            Vector3 position = target.position + invDistanceZ;

            target.transform.rotation = Quaternion.Euler(0, y, 0);
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    void HideCursor(bool isHiding)
    {
        if (isHiding)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void LateUpdate()
    {
        Orbit();
        Movement();
    }

}