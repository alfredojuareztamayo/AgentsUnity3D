
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This class defines various steering behaviors that agents can use for movement.
/// </summary>
public class SteeringBehaviour2 {
    /// <summary>
    /// Calculates steering force for seeking a target.
    /// </summary>
    /// <param name="agent">The agent's transform.</param>
    /// <param name="target">The target's transform.</param>
    /// <returns>The steering force to seek the target.</returns>
    public Vector3 Seek(Transform agent, Transform target) {
        StatsBasicAgent agentBasic = agent.GetComponent<StatsBasicAgent>();
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();

        Vector3 desiredVel = target.position - agent.position;
        desiredVel.Normalize();
        desiredVel *= agentBasic.MaxVel;
        Vector3 steering = desiredVel - agentRB.velocity;
        steering = Vector3.ClampMagnitude(steering, agentBasic.SteeringForce);
        steering /= agentRB.mass;
        steering =Vector3.ClampMagnitude(steering, agentBasic.SteeringForce);
        return steering;
    }

    public  void lookAt(Transform agent) {
        Vector3 copyVel = agent.GetComponent<Rigidbody>().velocity;
        float massCop = agent.GetComponent<Rigidbody>().mass;
        Vector3 agentRBVel = new Vector3(copyVel.x, copyVel.y, copyVel.z);
        agentRBVel.Normalize();
        agentRBVel /= massCop;
        //Vector3 toLook = new Vector3(
        //            steering.x + target.position.x,
        //            agentRB.position.y,
        //            steering.z + target.position.z
        //        );
        //agent.transform.LookAt(toLook);
        Vector3 toLook = new Vector3(
                    agentRBVel.x + agent.position.x,
                    agent.position.y,
                    agentRBVel.z + agent.position.z
                );
        
        agent.transform.LookAt(toLook);
    }

    /// <summary>
    /// Calculates steering force for fleeing from a target.
    /// </summary>
    /// <param name="agent">The agent's transform.</param>
    /// <param name="target">The target's transform.</param>
    /// <returns>The steering force to flee from the target.</returns>
    public Vector3 Flee(Transform agent, Transform target) {
        StatsBasicAgent agentBasic = agent.GetComponent<StatsBasicAgent>();
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();

        Vector3 desiredVel = agent.position - target.position; // Calculate the opposite direction
        desiredVel.Normalize();
        desiredVel *= agentBasic.MaxVel;
        Vector3 steering = desiredVel - agentRB.velocity;
        steering /= agentRB.mass;
        Vector3.ClampMagnitude(steering, agentBasic.SteeringForce);
        return steering;
    }

    /// <summary>
    /// Calculates steering force for wandering behavior.
    /// </summary>
    /// <param name="agent">The agent's transform.</param>
    /// <param name="wanderRadius">The radius for wandering.</param>
    /// <param name="wanderDistance">The distance for wandering.</param>
    /// <param name="wanderAngle">The wander angle.</param>
    /// <returns>The steering force for wandering.</returns>
    public Vector3 Wander(Transform agent, float wanderRadius, float wanderDistance, float wanderAngle) {
        StatsBasicAgent agentBasic = agent.GetComponent<StatsBasicAgent>();
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();

        // Modify the wander angle randomly
        wanderAngle += Random.Range(-1f, 1f) * agentBasic.WanderAngleDelta;

        // Calculate the target position within the wander circle
        Vector3 circleCenter = agent.position + agent.forward * wanderDistance;
        Vector3 displacement = wanderRadius * new Vector3(Mathf.Cos(wanderAngle), 0, Mathf.Sin(wanderAngle));
        Vector3 wanderTarget = circleCenter + displacement;

        // Calculate the direction to the target
        Vector3 desiredVel = wanderTarget - agent.position;
        desiredVel.Normalize();
        desiredVel *= agentBasic.MaxVel;

        // Calculate the steering direction
        Vector3 steering = desiredVel - agentRB.velocity;
        steering /= agentRB.mass;
        Vector3.ClampMagnitude(steering, agentBasic.SteeringForce);

        // Update the agent's orientation (you can uncomment this line if needed)
        // Vector3 toLook = new Vector3(
        //     steering.x + agent.position.x,
        //     agentRB.position.y,
        //     steering.z + agent.position.z
        // );
        // agent.transform.LookAt(toLook);

        return steering;
    }
}
