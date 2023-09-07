
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This class defines various steering behaviors that agents can use for movement.
/// </summary>
public static class SteeringBehaviour2 {
    /// <summary>
    /// Calculates steering force for seeking a target.
    /// </summary>
    /// <param name="agent">The agent's transform.</param>
    /// <param name="target">The target's transform.</param>
    /// <returns>The steering force to seek the target.</returns>
    public static Vector3 Seek(Transform agent, Vector3 target) {
        StatsBasicAgent agentBasic = agent.GetComponent<StatsBasicAgent>();
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();

        Vector3 desiredVel = target - agent.position;
        desiredVel.Normalize();
        desiredVel *= agentBasic.MaxVel;
        Vector3 steering = desiredVel - agentRB.velocity;
        steering = truncate(steering, agentBasic.SteeringForce);
        steering /= agentRB.mass;
        steering += agentRB.velocity;
        steering =truncate(steering, agentBasic.SteeringForce);
        steering.y =0;
        return steering;
    }


    /// <summary>
    /// Calculates steering force for fleeing from a target.
    /// </summary>
    /// <param name="agent">The agent's transform.</param>
    /// <param name="target">The target's transform.</param>
    /// <returns>The steering force to flee from the target.</returns>
    public static Vector3 Flee(Transform agent, Vector3 target) {
        return Seek(agent, target * -1);
    }
    public static void lookAt(Transform agent) {
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();
        Vector3 agentRBVel = agentRB.velocity;

        // Calcula la velocidad de rotación en función de la masa del agente
        float rotationSpeed = agentRBVel.magnitude / agentRB.mass;

        // Calcula la dirección a la que debe mirar el agente
        Vector3 toLook = agent.position + agentRBVel.normalized;

        // Aplica la rotación gradualmente en función de la velocidad de rotación
        Quaternion targetRotation = Quaternion.LookRotation(toLook - agent.position);
        agent.rotation = Quaternion.Slerp(agent.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    /// <summary>
    /// Calculates steering force for wandering behavior.
    /// </summary>
    /// <param name="agent">The agent's transform.</param>
    /// <param name="wanderRadius">The radius for wandering.</param>
    /// <param name="wanderDistance">The distance for wandering.</param>
    /// <param name="wanderAngle">The wander angle.</param>
    /// <returns>The steering force for wandering.</returns>
    public static Vector3 Wander(Transform agent, float wanderRadius, float wanderDistance, float wanderAngle) {
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

    private static Vector3 truncate(Vector3 vector, float maxValue) {
        if (vector.magnitude <= maxValue) { 
        return vector;
        }
        vector.Normalize();
        return vector*=maxValue;
    }
}
