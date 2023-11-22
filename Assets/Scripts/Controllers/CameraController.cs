using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 lastMousePosition = Vector3.zero;

    [SerializeField]
    private Transform rootScene;
    [SerializeField]
    private Transform mainCameraTransform;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    [Min(0.001f)]
    private float coefficient;
    [SerializeField]
    [Min(0.001f)]
    private float zoomCoefficient;

    private float targetImpulse;
    private float currentImpulse;
    private float zoom;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = lastMousePosition - Input.mousePosition;
            targetImpulse = -delta.x / coefficient;
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            targetImpulse = 0;
            lastMousePosition = Input.mousePosition;
        }

        currentImpulse = Mathf.LerpUnclamped(currentImpulse, targetImpulse, 10f * Time.deltaTime);

        if (mainCameraTransform.eulerAngles.y + currentImpulse > 180)
            currentImpulse = 180f - mainCameraTransform.eulerAngles.y;

        if (mainCameraTransform.eulerAngles.y + currentImpulse < 90)
            currentImpulse = 90f - mainCameraTransform.eulerAngles.y;

        mainCameraTransform.RotateAround(rootScene.position, Vector3.up, currentImpulse);
        mainCameraTransform.LookAt(rootScene.position + offset);

        if (Input.mouseScrollDelta.magnitude > 0f)
        {
            zoom -= Input.mouseScrollDelta.y / zoomCoefficient;
        }
        else
        {
            zoom = Mathf.Lerp(zoom, 0f, 10f * Time.deltaTime);
        }

        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, Mathf.Clamp(mainCamera.fieldOfView + zoom, 8f, 30f), 10f * Time.deltaTime);

    }
}
