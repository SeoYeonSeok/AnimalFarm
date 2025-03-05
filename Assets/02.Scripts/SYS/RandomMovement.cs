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
    public float moveRadius = 1.5f; // 랜덤 위치를 찾을 최대 반경

    [Header("MoveAnim")]
    public Animator moveAnim;
    private bool moveFlag = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        moveAnim = transform.GetChild(0).GetComponent<Animator>();
        MoveToRandomPosition(); // 시작 시 처음 이동
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
        for (int i = 0; i < 10; i++) // 10번 시도 (유효한 좌표를 찾지 못할 경우 대비)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius; // 원형 범위 내 무작위 위치 생성
            randomDirection += origin; // 현재 위치를 기준으로 오프셋 적용

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
            {
                return hit.position; // 유효한 NavMesh 위치 반환
            }
        }
        return origin; // 유효한 위치를 못 찾으면 현재 위치 반환
    }

    IEnumerator WaitAndMove()
    {
        moveFlag = true; // 대기 상태 ON

        if (moveAnim != null) moveAnim.SetBool("isMove", false);

        float waitTime = Random.Range(3, 9); // 3초 이상, 8초 이하의 랜덤 시간
        yield return new WaitForSeconds(waitTime);

        MoveToRandomPosition();
        moveFlag = false; // 대기 상태 OFF
    }
}