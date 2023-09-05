using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviour2
{
    public Vector3 Seek(Transform agent, Transform target)
    {
        StatsBasicAgent agentBasic = agent.GetComponent<StatsBasicAgent>();
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();

        Vector3 desiredVel = target.position - agent.position;
        desiredVel.Normalize();
        desiredVel *= agentBasic.MaxVel;
        Vector3 steering = desiredVel - agentRB.velocity;
        steering /= agentRB.mass;
        Vector3.ClampMagnitude(steering, agentBasic.SteeringForce);
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
        StatsBasicAgent agentBasic = agent.GetComponent<StatsBasicAgent>();
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();

        Vector3 desiredVel = agent.position - target.position; // Calcular la direcci�n opuesta
        desiredVel.Normalize();
        desiredVel *= agentBasic.MaxVel;
        Vector3 steering = desiredVel - agentRB.velocity;
        steering /= agentRB.mass;
        Vector3.ClampMagnitude(steering, agentBasic.SteeringForce);
        Vector3 toLook =
            new Vector3(steering.x + target.position.x,
                agentRB.position.y,
                steering.z + target.position.z
            );
        //steering.y = agentRB.velocity.y;
        agent.transform.LookAt(toLook);
        return steering;
    }
    public Vector3 Wander(Transform agent, float wanderRadius, float wanderDistance, float wanderAngle)
    {
        StatsBasicAgent agentBasic = agent.GetComponent<StatsBasicAgent>();
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();

        // Modificar el �ngulo de vagabundeo aleatoriamente
        
        wanderAngle += Random.Range(-1f, 1f) * agentBasic.WanderAngleDelta;

        // Calcular la posici�n del objetivo dentro del c�rculo de vagabundeo
        Vector3 circleCenter = agent.position + agent.forward * wanderDistance;
        Vector3 displacement = wanderRadius * new Vector3(Mathf.Cos(wanderAngle), 0, Mathf.Sin(wanderAngle));
        Vector3 wanderTarget = circleCenter + displacement;

        // Calcular la direcci�n hacia el objetivo
        Vector3 desiredVel = wanderTarget - agent.position;
        desiredVel.Normalize();
        desiredVel *= agentBasic.MaxVel;

        // Calcular la direcci�n del steering
        Vector3 steering = desiredVel - agentRB.velocity;
        steering /= agentRB.mass;
        Vector3.ClampMagnitude(steering, agentBasic.SteeringForce);

        // Actualizar la orientaci�n del agente
       // Vector3 toLook = new Vector3(steering.x + agent.position.x, agentRB.position.y, steering.z + agent.position.z);
        //agent.transform.LookAt(toLook);

        return steering;
    }
}
