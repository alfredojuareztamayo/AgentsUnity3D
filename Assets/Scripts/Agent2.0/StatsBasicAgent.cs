using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsBasicAgent : MonoBehaviour
{
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

    public float Vida
    {
        get { return vida; }
        set
        {
            if (value >= 0)
                vida = value;
            else
                Debug.LogError("La vida no puede ser un valor negativo");
        }
    }

    public float Armadura
    {
        get { return armadura; }
        set
        {
            if (value >= 0)
                armadura = value;
            else
                Debug.LogError("La armadura no puede ser un valor negativo");
        }
    }

    public float Velocidad
    {
        get { return velocidad; }
        set
        {
            if (value >= 0)
                velocidad = value;
            else
                Debug.LogError("La velocidad no puede ser un valor negativo");
        }
    }

    public float Fuerza
    {
        get { return fuerza; }
        set
        {
            if (value >= 0)
                fuerza = value;
            else
                Debug.LogError("La fuerza no puede ser un valor negativo");
        }
    }

    public float MaxVel
    {
        get { return maxVel; }
        set { maxVel = value; }
    }

    public float SteeringForce
    {
        get { return steeringForce; }
        set { steeringForce = value; }
    }
    public float WanderAngleDelta
    {
        get { return wanderAngleDelta; }
        set
        {
            wanderAngleDelta = value;
        }
    }
    public TypeAgent2 TypeAgent
    {
        get { return typeAgent; }
        set { typeAgent = value;}
    }
    public Transform EyePerception
    {
        get { return m_eyePerception; }
        set { m_eyePerception = value;}
    }
    public float EyesPerceptionRad
    {
        get { return m_eyesPerceptionRad; }
        set { m_eyesPerceptionRad = value; }
    }

    public bool ProtectedAttack
    {
        get { return protectedAttack; }
        set { protectedAttack = value; }
    }

    public void MeAtacan(Color color)
    {
        GetComponent<Renderer>().material.color = color;
        Vida -= .1f;
    }
}
