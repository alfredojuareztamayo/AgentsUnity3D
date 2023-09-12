using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the basic attributes and properties of an agent.
/// </summary>
public class AgentBasic2 {
    private float vida;
    private float armadura;
    private float velocidad;
    private float fuerza;
    private float maxVel;
    private float steeringForce;
    private float wanderAngleDelta;

    /// <summary>
    /// Gets or sets the agent's health points (vida).
    /// </summary>
    public float Vida {
        get { return vida; }
        set {
            
                vida = value;
          
               // Debug.LogError("La vida no puede ser un valor negativo");
        }
    }

    /// <summary>
    /// Gets or sets the agent's armor points (armadura).
    /// </summary>
    public float Armadura {
        get { return armadura; }
        set {
           
                armadura = value;
           
                //Debug.LogError("La armadura no puede ser un valor negativo");
        }
    }

    /// <summary>
    /// Gets or sets the agent's movement speed (velocidad).
    /// </summary>
    public float Velocidad {
        get { return velocidad; }
        set {
            
                velocidad = value;
           
                //Debug.LogError("La velocidad no puede ser un valor negativo");
        }
    }

    /// <summary>
    /// Gets or sets the agent's attack strength (fuerza).
    /// </summary>
    public float Fuerza {
        get { return fuerza; }
        set {
            
                fuerza = value;
           
               // Debug.LogError("La fuerza no puede ser un valor negativo");
        }
    }

    /// <summary>
    /// Gets or sets the agent's maximum velocity (maxVel).
    /// </summary>
    public float MaxVel {
        get { return maxVel; }
        set { maxVel = value; }
    }

    /// <summary>
    /// Gets or sets the agent's steering force (steeringForce).
    /// </summary>
    public float SteeringForce {
        get { return steeringForce; }
        set { steeringForce = value; }
    }

    /// <summary>
    /// Gets or sets the agent's wander angle delta (wanderAngleDelta).
    /// </summary>
    public float WanderAngleDelta {
        get { return wanderAngleDelta; }
        set { wanderAngleDelta = value; }
    }

    /// <summary>
    /// Initializes a new instance of the AgentBasic2 class based on the agent type.
    /// </summary>
    /// <param name="typeAgent">The type of agent to initialize.</param>
    public AgentBasic2(TypeAgent2 typeAgent) {
        switch (typeAgent) {
            case TypeAgent2.Melee:
                Vida = 180f;
                Armadura = 20f;
                Velocidad = 40f;
                Fuerza = 160f;
                MaxVel = 5f;
                SteeringForce = 5f;
                break;
            case TypeAgent2.Shooter:
                Vida = 120f;
                Armadura = 40f;
                Velocidad = 160f;
                Fuerza = 80;
                MaxVel = 6f;
                SteeringForce = 6f;
                break;
            case TypeAgent2.Tank:
                Vida = 200f;
                Armadura = 180f;
                Velocidad = 20f;
                Fuerza = 140f;
                MaxVel = 4f;
                SteeringForce = 5f;
                break;
            case TypeAgent2.Vigila:
                Vida = 120f;
                Armadura = 40f;
                Velocidad = 160f;
                Fuerza = 80;
                MaxVel = 3f;
                SteeringForce = 6f;
                break;
        }
    }
}

/// <summary>
/// Represents the types of agents.
/// </summary>
public enum TypeAgent2 {
    Melee,
    Tank,
    Shooter,
    Vigila
}
public enum MovementAgent {
    None,
    Seek,
    Flee,
    Wander,
    Pursuit,
    Arrival
}
public enum ActionAgent {
    None,
    Heal,
    Attack,
    Running
}
public enum PhysicalState {
    Afraid,
    Tired,
    Happy
}
