using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 0.1f; // 이동 속도 조절
    private Vector3 dragStartPos;
    private Vector3 cameraStartPos;

    private float minX_Farm = -10f, maxX_Farm = 10f; // X축 이동 제한
    private float minZ_Farm = -2.5f, maxZ_Farm = 2.5f; // Z축 이동 제한

    private float minX_Mount = -3.75f, maxX_Mount = 3.75f; // X축 이동 제한
    private float minY_Mount = -1f, maxY_Mount = 1f; // Z축 이동 제한

    [Header("Other Scripts")]
    public GameMgr gameMgr;

    void Update()
    {
        //if (gameMgr.GetMainCamStatus() == 1) HandleCameraDrag_Farm();
        if (gameMgr.GetMainCamStatus() == 2) HandleCameraDrag_Mountain();
    }

    void HandleCameraDrag_Farm()
    {
        if (Input.GetMouseButtonDown(0)) // 터치 시작 (마우스 클릭 포함)
        {
            dragStartPos = Input.mousePosition;
            cameraStartPos = transform.localPosition;
        }
        else if (Input.GetMouseButton(0)) // 드래그 중
        {
            Vector3 dragCurrentPos = Input.mousePosition;
            Vector3 dragDelta = dragCurrentPos - dragStartPos;

            // 입력값을 반전시켜 카메라 이동
            float moveX = -dragDelta.x * moveSpeed * Time.deltaTime;
            float moveZ = -dragDelta.y * moveSpeed * Time.deltaTime;

            Vector3 newPosition = cameraStartPos + new Vector3(moveX, 0, moveZ);

            // 이동 범위 제한 적용
            newPosition.x = Mathf.Clamp(newPosition.x, minX_Farm, maxX_Farm);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ_Farm, maxZ_Farm);

            transform.localPosition = newPosition;
        }
    }

    void HandleCameraDrag_Mountain()
    {
        if (Input.GetMouseButtonDown(0)) // 터치 시작 (마우스 클릭 포함)
        {
            dragStartPos = Input.mousePosition;
            cameraStartPos = transform.localPosition;
        }
        else if (Input.GetMouseButton(0)) // 드래그 중
        {
            Vector3 dragCurrentPos = Input.mousePosition;
            Vector3 dragDelta = dragCurrentPos - dragStartPos;

            // 입력값을 반전시켜 카메라 이동
            float moveX = -dragDelta.x * moveSpeed * Time.deltaTime;
            float moveY = -dragDelta.y * moveSpeed * Time.deltaTime;

            Vector3 newPosition = cameraStartPos + new Vector3(moveX, moveY, 0);

            // 이동 범위 제한 적용
            newPosition.x = Mathf.Clamp(newPosition.x, minX_Mount, maxX_Mount);
            newPosition.y = Mathf.Clamp(newPosition.y, minY_Mount, maxY_Mount);

            transform.localPosition = newPosition;
        }
    }

}
