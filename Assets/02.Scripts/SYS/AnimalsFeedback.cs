using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsFeedback : MonoBehaviour
{
    [Header("Feedback Animator")]
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckTouchOrClick(Input.mousePosition);
        }

        /*
        if (Input.touchCount > 0) // 모바일 터치
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                CheckTouchOrClick(touch.position);
            }
        }
        */
    }

    void CheckTouchOrClick(Vector2 position)
    {        
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // 3D 오브젝트 감지 (Physics)
        {
            if (hit.collider.gameObject == gameObject) // 현재 오브젝트가 맞으면
            {
                Debug.Log("Touched");
                anim.SetTrigger("Touched"); // Touched 트리거 발동
            }
        }
    }

}
