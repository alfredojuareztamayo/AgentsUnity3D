using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviours{

    public Vector3 Seek(Transform agent, Transform target) {
        BasicAgent agentBasic = agent.GetComponent<BasicAgent>();
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();

        Vector3 desiredVel = target.position - agent.position;
        desiredVel.Normalize();
        desiredVel *= agentBasic.GetMaxVel();
        Vector3 steering = desiredVel - agentRB.velocity;
        steering /= agentRB.mass;
        Vector3.ClampMagnitude(steering, agentBasic.GetMaxSteeringForce());
        Vector3 toLook = 
            new Vector3(steering.x + target.position.x,
                agentRB.position.y, 
                steering.z + target.position.z
            );
        //steering.y = agentRB.velocity.y;
        agent.transform.LookAt(toLook);
        return steering;
    }

    public Vector3 Flee(Transform agent, Transform target)
    {
        Vector3 flee = Seek(agent, target) * -1;
        return flee;
    }


    //public Vector3 Wander(Transform agent)
    //{
    //   // BasicAgent agentBasic = agent.GetComponent<BasicAgent>();
    //   // Rigidbody agentRB = agent.GetComponent<Rigidbody>();
    //   // Vector3 desiredVel = agent.position;
    //   // desiredVel.Normalize();
    //   // desiredVel *= agentBasic.GetMaxVel();
    //   //// desiredVel *= 10f;
    //   // desiredVel += agent.position;
    //   // Vector3 randomWheel = new Vector3(Random.Range(-1, 1), agent.position.y, Random.Range(-1, 1));
    //   // randomWheel.Normalize();
    //   //// randomWheel *= 4f;
    //   // randomWheel += desiredVel;
    //   // return randomWheel;
    //}
    
}//End class SteeringBehaviours
