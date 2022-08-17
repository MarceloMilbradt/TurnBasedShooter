using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    private const float MIN_FOLLOW_Y_OFFSET = 2F; 
    private const float MAX_FOLLOW_Y_OFFSET = 12F; 
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleZoom()
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        float zoomAmount = 1f;
        var followOffset = transposer.m_FollowOffset;
        if (Input.mouseScrollDelta.y > 0f)
        {
            followOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0f)
        {
            followOffset.y += zoomAmount;
        }
        float zoomSpeed = 5f;
        followOffset.y = Mathf.Clamp(followOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }
        float moveSpeed = 10f;
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveSpeed * Time.deltaTime * moveVector;
    }

    private void HandleRotation()
    {
        float rotationSpeed = 100f;
        Vector3 rotationVector = Vector3.zero;
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }
}
