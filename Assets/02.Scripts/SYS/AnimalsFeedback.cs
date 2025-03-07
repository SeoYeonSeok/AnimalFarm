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
        if (Input.touchCount > 0) // ����� ��ġ
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

        if (Physics.Raycast(ray, out hit)) // 3D ������Ʈ ���� (Physics)
        {
            if (hit.collider.gameObject == gameObject) // ���� ������Ʈ�� ������
            {
                Debug.Log("Touched");
                anim.SetTrigger("Touched"); // Touched Ʈ���� �ߵ�
            }
        }
    }

}
