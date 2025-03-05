using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class RandomMovement : MonoBehaviour 
{
    public NavMeshAgent agent;
    public float range;

    public Transform centrePoint;

    private bool turnMove = false;

    //private Animator animal_MoveAnim;

    [Header("MoveRadius")]
    public float moveRadius = 1.5f; // ���� ��ġ�� ã�� �ִ� �ݰ�

    [Header("MoveAnim")]
    public Animator moveAnim;
    private bool moveFlag = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        moveAnim = transform.GetChild(0).GetComponent<Animator>();
        MoveToRandomPosition(); // ���� �� ó�� �̵�
    }


    void Update()
    {
        if (!moveFlag && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(WaitAndMove());          
        }
    }

    public void MoveToRandomPosition()
    {
        Vector3 randomPosition = GetRandomNavMeshPosition(transform.position, moveRadius);
        agent.SetDestination(randomPosition);
        if (moveAnim != null) moveAnim.SetBool("isMove", true);
    }   

    Vector3 GetRandomNavMeshPosition(Vector3 origin, float radius)
    {
        for (int i = 0; i < 10; i++) // 10�� �õ� (��ȿ�� ��ǥ�� ã�� ���� ��� ���)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius; // ���� ���� �� ������ ��ġ ����
            randomDirection += origin; // ���� ��ġ�� �������� ������ ����

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
            {
                return hit.position; // ��ȿ�� NavMesh ��ġ ��ȯ
            }
        }
        return origin; // ��ȿ�� ��ġ�� �� ã���� ���� ��ġ ��ȯ
    }

    IEnumerator WaitAndMove()
    {
        moveFlag = true; // ��� ���� ON

        if (moveAnim != null) moveAnim.SetBool("isMove", false);

        float waitTime = Random.Range(3, 9); // 3�� �̻�, 8�� ������ ���� �ð�
        yield return new WaitForSeconds(waitTime);

        MoveToRandomPosition();
        moveFlag = false; // ��� ���� OFF
    }
}