using System.Collections;
using System.Collections.Generic;

using UnityEngine;



/// <summary>
/// This class represents the basic statistics of an agent in the game.
/// </summary>
public class StatsBasicAgent : MonoBehaviour {
    [SerializeField] float vida;
    [SerializeField] float armadura;
    [SerializeField] float velocidad;
    [SerializeField] float fuerza;
    [SerializeField] float maxVel;
    [SerializeField] float steeringForce;
    [SerializeField] float wanderAngleDelta;
    [SerializeField] TypeAgent2 typeAgent;
    [SerializeField] Transform m_eyePerception;
    [SerializeField] float m_eyesPerceptionRad;
    [SerializeField] bool protectedAttack;

    /// <summary>
    /// Gets or sets the health points of the agent.
    /// </summary>
    public float Vida {
        get { return vida; }
        set {
           
                vida = value;
            
               // Debug.LogError("Health cannot be a negative value.");
        }
    }

    /// <summary>
    /// Gets or sets the armor points of the agent.
    /// </summary>
    public float Armadura {
        get { return armadura; }
        set {
           
                armadura = value;
            
              //  Debug.LogError("Armor cannot be a negative value.");
        }
    }

    /// <summary>
    /// Gets or sets the movement speed of the agent.
    /// </summary>
    public float Velocidad {
        get { return velocidad; }
        set {
            
                velocidad = value;
            
               // Debug.LogError("Speed cannot be a negative value.");
        }
    }

    /// <summary>
    /// Gets or sets the strength of the agent.
    /// </summary>
    public float Fuerza {
        get { return fuerza; }
        set {
            
                fuerza = value;
           
        }
    }

    /// <summary>
    /// Gets or sets the maximum velocity of the agent.
    /// </summary>
    public float MaxVel {
        get { return maxVel; }
        set { maxVel = value; }
    }

    /// <summary>
    /// Gets or sets the steering force of the agent.
    /// </summary>
    public float SteeringForce {
        get { return steeringForce; }
        set { steeringForce = value; }
    }

    /// <summary>
    /// Gets or sets the delta for wander angle.
    /// </summary>
    public float WanderAngleDelta {
        get { return wanderAngleDelta; }
        set { wanderAngleDelta = value; }
    }

    /// <summary>
    /// Gets or sets the type of the agent.
    /// </summary>
    public TypeAgent2 TypeAgent {
        get { return typeAgent; }
        set { typeAgent = value; }
    }

    /// <summary>
    /// Gets or sets the eye perception transform of the agent.
    /// </summary>
    public Transform EyePerception {
        get { return m_eyePerception; }
        set { m_eyePerception = value; }
    }

    /// <summary>
    /// Gets or sets the radius of eye perception.
    /// </summary>
    public float EyesPerceptionRad {
        get { return m_eyesPerceptionRad; }
        set { m_eyesPerceptionRad = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the agent is protected during an attack.
    /// </summary>
    public bool ProtectedAttack {
        get { return protectedAttack; }
        set { protectedAttack = value; }
    }

    /// <summary>
    /// Method called when the agent is attacked, changing its color and reducing health.
    /// </summary>
    /// <param name="color">The color to change to upon being attacked.</param>
    public void MeAtacan(Color color) {
        GetComponent<Renderer>().material.color = color;
        Vida -= 0.1f;
    }
}
