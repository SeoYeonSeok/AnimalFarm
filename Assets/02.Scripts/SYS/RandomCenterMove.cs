using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCenterMove : MonoBehaviour
{
    public float xRange = 4.5f;  
    public float zRange = 2.5f;  
    public float minTime = 4f;  
    public float maxTime = 8f;  

    void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {            
            Vector3 newPos = new Vector3(
                Random.Range(-xRange, xRange),
                transform.localPosition.y,
                Random.Range(-zRange, zRange)
            );
            
            transform.localPosition = newPos;
            
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

}
