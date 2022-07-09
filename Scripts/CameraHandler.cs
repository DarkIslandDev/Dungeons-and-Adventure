using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraHandler : MonoBehaviour
{
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform pivotTransform;
    
    private Transform myTransform;
    private Vector3 cameraTransformPosition;
    public LayerMask ignoreLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public static CameraHandler instance;

    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    public float minPivot = -35;
    public float maxPivot = 35;
    
    [Header("Коллизия камеры")]
    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffSet = 0.2f;
    public float minCollisionOffSet = 0.2f;

    [Header("Зум камеры")] 
    public float stepSize = 2f;
    public float zoomDampening = 7.5f;
    public float minZoom = 2f;
    public float maxZoom = 12f;
    public float zoomSpeed = 2f;
    [HideInInspector]
    public float zoomHeight;
    
    private void Awake()
    {
        instance = this;
        myTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        // Cursor.lockState = CursorLockMode.Locked;
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        zoomHeight = cameraTransform.localPosition.y;
        zoomHeight = 2.3f;
    }

    private void Update()
    {
        UpdateCameraPosition();
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        myTransform.position = targetPosition;
        
        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        lookAngle += (mouseXInput * lookSpeed) / delta;
        pivotAngle -= (mouseYInput * pivotSpeed) / delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        myTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);
        pivotTransform.localRotation = targetRotation;
    }

    private void HandleCameraCollision(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - pivotTransform.position;
        direction.Normalize();

        if(Physics.SphereCast(pivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
        {
            float dis = Vector3.Distance(pivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffSet);
        }

        if(Mathf.Abs(targetPosition) < minCollisionOffSet)
        {
            targetPosition = -minCollisionOffSet;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }

    public void Zooming(float value)
    {
        zoomHeight = pivotTransform.localPosition.y + value * stepSize;

        if (zoomHeight < minZoom)
        {
            zoomHeight = minZoom;
            minPivot = -35;
        }
        else if(zoomHeight > maxZoom)
        {
            zoomHeight = maxZoom;
            minPivot = 35;
        }
        else
        {
            minPivot = 15;
        }
    }

    public void UpdateCameraPosition()
    {
        Vector3 zoomTarget = 
            new Vector3(pivotTransform.localPosition.x, zoomHeight, pivotTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - pivotTransform.localPosition.y) * Vector3.forward;
        //
        pivotTransform.localPosition =
            Vector3.Lerp
                (pivotTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        //
        // pivotTransform.LookAt(this.transform);
    }
} 
