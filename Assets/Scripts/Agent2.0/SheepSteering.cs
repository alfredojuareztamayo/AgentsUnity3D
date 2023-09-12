using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSteering : MonoBehaviour
{
    [SerializeField] Transform leader;
    [SerializeField] float followDist;
    void Start()
    {
        followDist = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(SteeringBehaviour2.LeaderFollowing(transform,leader,followDist));
    }
}
