using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviours{

    public Vector3 seek(Transform agent, Transform target) {
        BasicAgent agentBasic = agent.GetComponent<BasicAgent>();
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();

        Vector3 desiredVel = target.position - agent.position;
        desiredVel.Normalize();
        desiredVel *= agentBasic.getMaxVel();
        Vector3 steering = desiredVel - agentRB.velocity;
        steering /= agentRB.mass;
        Vector3.ClampMagnitude(steering, agentBasic.getMaxSteeringForce());
        steering.y = agentRB.velocity.y;
        agent.transform.LookAt(steering);
        return steering;
    }

}//End class SteeringBehaviours
