using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    Vector3 posA;
    Vector3 posB;
    Vector3 nextPos;

    [SerializeField]
    private float speed;

    private float timeToWait;

    [SerializeField]
    private Transform childTransform;

    [SerializeField]
    private Transform transformB;

    // Use this for initialization
    void Start () {
        posA = childTransform.localPosition;
        posB = transformB.localPosition;
        nextPos = posB;
        timeToWait = 4;
    }
    
    // Update is called once per frame
    void Update () {
        Move();
    }

    private void Move()
    {
        childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, nextPos, speed * Time.deltaTime);
        if (Vector3.Distance(childTransform.localPosition, nextPos) <= 0.1)
        {
            timeToWait -= Time.deltaTime;
            if(timeToWait <= 0)
            {
                ChangeDestination();
                timeToWait = 4;
            }
            
        }

    }

    private void ChangeDestination()
    {
        nextPos = nextPos != posA ? posA : posB;
    }
}
