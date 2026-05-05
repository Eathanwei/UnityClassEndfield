using UnityEngine;
using System.Collections.Generic;

public class HW2_camera : MonoBehaviour
{
    int mainControl = 0;
    float fMX, fMY;
    float mouseHorizontalSensitivity = 300.0f;
    float mouseVerticalSensitivity = 200.0f;
    [SerializeField] float rotateVerticalAngle = 0.0f;
    [SerializeField] float rotateVerticalAngleUpBounded = 89f;
    float rotateVerticalAngleDownBounded = 0f;
    [SerializeField] List<Transform> followTargets;
    Vector3 xzDirection;
    Vector3 xyzDirection;

    [SerializeField] float cameraRadius = 1f;
    float targetDistance = 5f;
    [SerializeField] float targetDistanceCurrent = 5f;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float minDistance = 0.65f;
    [SerializeField] float heightDistanceScale = 1f;
    float farDamping = 1f;
    [SerializeField] LayerMask obstacleLayers;

    public bool showGizmos = true;
    public Color gizmoColor = Color.yellow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xzDirection = transform.position - followTargets[mainControl].position;
        xzDirection.y = 0;
        xzDirection.Normalize();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        fMX = Input.GetAxis("Mouse X");
        fMY = Input.GetAxis("Mouse Y");

        if (Cursor.lockState == CursorLockMode.Locked && !InputManager.isUsingUI)
        {
            rotateVerticalAngle += fMY * mouseVerticalSensitivity * Time.deltaTime;
        }
        if (rotateVerticalAngle > rotateVerticalAngleUpBounded)
        {
            rotateVerticalAngle = rotateVerticalAngleUpBounded;
        }
        else if (rotateVerticalAngle < rotateVerticalAngleDownBounded)
        {
            rotateVerticalAngle = rotateVerticalAngleDownBounded;
        }
        if(Cursor.lockState == CursorLockMode.Locked && !InputManager.isUsingUI)
        {
            xzDirection = Quaternion.AngleAxis(fMX * mouseHorizontalSensitivity * Time.deltaTime, Vector3.up) * xzDirection;
            xyzDirection = Quaternion.AngleAxis(rotateVerticalAngle, Vector3.Cross(xzDirection, Vector3.up)) * xzDirection;
            xyzDirection = xyzDirection.normalized;
        }

        RaycastHit hit;
        targetDistance = maxDistance * (1 + heightDistanceScale * rotateVerticalAngle / rotateVerticalAngleUpBounded);
        //if (Physics.Raycast(followTarget.position, xyzDirection, out hit, maxDistance, obstacleLayers))
        if (Physics.SphereCast(followTargets[mainControl].position, cameraRadius, xyzDirection, out hit, targetDistance, obstacleLayers))
        {
            targetDistance = Mathf.Clamp(hit.distance - 0.2f, minDistance, targetDistance);
        }
        if (targetDistance > targetDistanceCurrent)
        {
            targetDistanceCurrent = Mathf.Lerp(targetDistanceCurrent, targetDistance, Time.deltaTime * farDamping);
        }
        else
        {
            targetDistanceCurrent = targetDistance;
        }
        transform.position = followTargets[mainControl].position + xyzDirection * targetDistanceCurrent;


        //transform.position = followTarget.position + xyzDirection.normalized * distanceToTarget;
        //transform.Translate(0, transform.position.y, 0, Space.World);
        transform.LookAt(followTargets[mainControl]);
    }
    private void OnDrawGizmos()
    {
        if (!showGizmos || followTargets[mainControl] == null) return;

        Gizmos.color = gizmoColor;

        // 1. 畫出從目標（角色）到相機的射線路徑
        Gizmos.DrawLine(followTargets[mainControl].position, transform.position);

        // 2. 在相機當前位置畫出一個球體，代表避障的「厚度」
        Gizmos.DrawWireSphere(transform.position, cameraRadius);

        // 3. (選填) 畫出預期最大距離的終點
        Vector3 maxPos = followTargets[mainControl].position + xyzDirection.normalized * maxDistance;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(maxPos, 0.1f);
    }
    public void MainControl(int mainControl)
    {
        this.mainControl = mainControl;
    }
}
