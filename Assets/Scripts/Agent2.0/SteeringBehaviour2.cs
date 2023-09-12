
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
    /// Calculates steering force for arriving at a target.
    /// </summary>
    /// <param name="agent">The agent's transform.</param>
    /// <param name="target">The target's transform.</param>
    /// <param name="arrivalRadius">The radius for arriving.</param>
    /// <returns>The steering force to arrive at the target.</returns>
    //public static Vector3 Arrival(Transform agent, Vector3 target)
    //{
    //    StatsBasicAgent agentBasic = agent.GetComponent<StatsBasicAgent>();
    //    Rigidbody agentRB = agent.GetComponent<Rigidbody>();

    //    Vector3 desiredVel = target - agent.position;
    //    float distance = desiredVel.magnitude;

    //    // Check if the agent has already arrived
    //    if (distance < agentBasic.ArrivalRadius)
    //    {
    //        return Vector3.zero; // No steering needed if already arrived
    //    }

    //    desiredVel.Normalize();

    //    // Calculate the desired velocity based on the arrival radius
    //    if (distance < agentBasic.ArrivalRadius)
    //    {
    //        desiredVel *= agentBasic.MaxVel * (distance / agentBasic.ArrivalRadius);
    //    }
    //    else
    //    {
    //        desiredVel *= agentBasic.MaxVel;
    //    }

    //    Vector3 steering = desiredVel - agentRB.velocity;
    //    steering = truncate(steering, agentBasic.SteeringForce);
    //    steering /= agentRB.mass;
    //    steering.y = 0;

    //    return steering;
    //}

    public static Vector3 Arrival(Transform agent, Vector3 target)
    {
        StatsBasicAgent agentBasic = agent.GetComponent<StatsBasicAgent>();
        // Check if the agent has already arrived
        if (Vector3.Distance(agent.position, target) < agentBasic.ArrivalRadius)
        {
            return Vector3.zero; // No steering needed if already arrived
        }
        // Calculate the desired velocity using Seek
        Vector3 seekForce = Seek(agent, target);
        float distance = seekForce.magnitude;
        // Adjust the desired velocity based on the arrival radius
        if (distance < agentBasic.ArrivalRadius)
        {
            seekForce *= distance / agentBasic.ArrivalRadius;
        }

        return seekForce;
    }

    /// <summary>
    /// Calculates steering force for fleeing from a target.
    /// </summary>
    /// <param name="agent">The agent's transform.</param>
    /// <param name="target">The target's transform.</param>
    /// <returns>The steering force to flee from the target.</returns>
    //public static Vector3 Flee(Transform agent, Vector3 target) {
    //    return Seek(agent, target * -1);
    //}
    public static Vector3 Flee(Transform agent, Vector3 target)
    {
        StatsBasicAgent agentBasic = agent.GetComponent<StatsBasicAgent>();
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();

        Vector3 desiredVel = agent.position - target; // Calcular la dirección opuesta
       // Vector3 desiredVel = target - agent.position;
        desiredVel.Normalize();
        desiredVel *= agentBasic.MaxVel;
        Vector3 steering = desiredVel - agentRB.velocity;
        steering = truncate(steering, agentBasic.SteeringForce);
        steering /= agentRB.mass;
        steering += agentRB.velocity;
        steering = truncate(steering, agentBasic.SteeringForce);
        steering.y = 0;
        return steering;
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
    /// <summary>
    /// Calculates steering force for pursuit behavior.
    /// </summary>
    /// <param name="agent">The agent's transform.</param>
    /// <param name="target">The target's transform.</param>
    /// <returns>The steering force for pursuit.</returns>
    public static Vector3 Pursuit(Transform agent, Transform target)
    {
        StatsBasicAgent agentBasic = agent.GetComponent<StatsBasicAgent>();
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();

        // Initialize variables
        Vector3 steering = Vector3.zero;
        float maxTimePrediction = 2.0f; // Maximum time to predict ahead

        while (maxTimePrediction > 0)
        {
            // Calculate the direction to the target's future position
            Vector3 toTarget = target.position - agent.position;
            float distance = toTarget.magnitude;
            float timePrediction = distance / agentBasic.MaxVel;

            // Limit the time prediction to a maximum value
            timePrediction = Mathf.Min(timePrediction, maxTimePrediction);

            // Calculate the future position of the target
            Vector3 futureTargetPosition = target.position + target.GetComponent<Rigidbody>().velocity * timePrediction;

            // Calculate the desired velocity toward the future target position
            Vector3 desiredVel = futureTargetPosition - agent.position;
            desiredVel.Normalize();
            desiredVel *= agentBasic.MaxVel;

            // Calculate the steering direction
            steering = desiredVel - agentRB.velocity;
            steering = truncate(steering, agentBasic.SteeringForce);
            steering /= agentRB.mass;
            steering.y = 0;

            // Check if the agent is within a certain distance of the future target
            if (distance < agentBasic.MaxVel * timePrediction)
            {
                return steering; // The agent is close enough to the future target
            }

            // Decrease the maximum time prediction for the next iteration
            maxTimePrediction -= 0.1f;
        }

        return steering; // If no suitable future position found, return the last calculated steering
    }
    /// <summary>
    /// Calculates steering force for path following behavior.
    /// </summary>
    /// <param name="path">Array of waypoints defining the path.</param>
    /// <param name="warrior">The agent's transform.</param>
    /// <param name="currentWaypointIndex">The current waypoint index.</param>
    /// <returns>The steering force for path following.</returns>
    public static Vector3 PathFollow(Vector3[] path, Transform warrior, int currentWaypointIndex)
    {
      
        float speed = 3f;
        Vector3 currentWaypoint = path[currentWaypointIndex];

        // Calcular la dirección hacia el punto de destino
        Vector3 direction = currentWaypoint - warrior.position;
        direction.Normalize();

        // Moverse hacia el punto de destino
      Vector3 newTransform = direction * speed * Time.deltaTime;

        // Verificar si el agente está lo suficientemente cerca del punto actual
       
        return newTransform;
    }
    /// <summary>
    /// Calculates steering force for leader following behavior.
    /// </summary>
    /// <param name="sheep">The agent's transform (follower).</param>
    /// <param name="leader">The leader's transform.</param>
    /// <param name="followDistance">The desired follow distance behind the leader.</param>
    /// <returns>The steering force for leader following.</returns>
    public static Vector3 LeaderFollowing(Transform sheep,Transform leader, float followDistance)
    {
        // Calcular la posición objetivo a seguir basada en la posición del líder
        Vector3 targetPosition = leader.position - leader.forward * followDistance;

        // Calcular la dirección hacia la posición objetivo
        Vector3 direction = targetPosition - sheep.position;
        direction.Normalize();

        // Moverse hacia la posición objetivo
        float speed = 2.0f; // Velocidad de movimiento del seguidor
       Vector3 sheepPath = direction * speed * Time.deltaTime;
        return sheepPath;
    }
    /// <summary>
    /// Truncates a vector to a maximum magnitude.
    /// </summary>
    /// <param name="vector">The vector to truncate.</param>
    /// <param name="maxValue">The maximum magnitude.</param>
    /// <returns>The truncated vector.</returns>
    private static Vector3 truncate(Vector3 vector, float maxValue) {
        if (vector.magnitude <= maxValue) { 
        return vector;
        }
        vector.Normalize();
        return vector*=maxValue;
    }
}
