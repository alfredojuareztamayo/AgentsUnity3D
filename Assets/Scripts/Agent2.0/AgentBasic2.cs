using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBasic2
{
    private float vida;
    private float armadura;
    private float velocidad;
    private float fuerza;
    private float maxVel;
    private float steeringForce;
    private float wanderAngleDelta;
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

    public AgentBasic2(TypeAgent2 typeAgent)
    {
        switch (typeAgent)
        {
            case TypeAgent2.Melee:
                Vida = 180f;
                Armadura = 20f;
                Velocidad = 40f;
                Fuerza = 160f;
                maxVel = 5f;
                steeringForce = 5f;
                break;
            case TypeAgent2.Shooter:
                Vida = 120f;
                Armadura = 40f;
                Velocidad = 160f;
                Fuerza = 80;
                maxVel = 6f;
                steeringForce = 6f;
                break;
            case TypeAgent2.Tank:
                Vida = 200f;
                Armadura = 180f;
                Velocidad = 20f;
                Fuerza = 140f;
                maxVel = 4f;
                steeringForce = 5f;
                break;


        }     
    }  
     
}
public enum TypeAgent2
{
    Melee,
    Tank,
    Shooter
}
public enum MovementAgent
{
    None,
    Seek,
    Flee
}
public enum ActionAgent
{
    None,
    Heal,
    Attack,
    Running
}
public enum PhysicalState
{
    Afraid,
    Tired,
    Happy
}