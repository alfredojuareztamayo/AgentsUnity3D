using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicAgent : MonoBehaviour {

    [SerializeField] protected float m_maxVel, m_maxSteeringForce;
    [SerializeField] protected float m_life, m_armor, m_strength;
    [SerializeField] protected float m_eyesPerceptionRad, m_earPerceptionRad;
    [SerializeField] protected Transform m_eyesPerceptionPos, m_earPerceptionPos;
    [SerializeField] protected bool m_justOneTarget;
    [SerializeField] protected Transform m_target;
    private AgentState m_agentState;
    private TypeAgent m_typeAgent;
    private TeamAgent m_teamAgent;
    public float GetMaxVel() {
        return m_maxVel;
    }
    public float GetMaxSteeringForce() {
        return m_maxSteeringForce;
    }
   public void ChangeAgentStaet(AgentState newState) {
        if(newState == m_agentState) {
            return;
        }
        m_agentState = newState;
        switch(newState) {
            case AgentState.None:
                break;
            case AgentState.Seeking: 
                break;
            case AgentState.Fleing: 
                break;
            case AgentState.Wandering: 
                break;

        }
    }
    public AgentState GetAgentState() {
        return m_agentState;
    }

    public TypeAgent GetTypeAgent()
    {
        return m_typeAgent;
    }
    public void SetTypeAgent(TypeAgent newTypeAgent)
    {
        if(m_typeAgent == newTypeAgent)
        {
            return;
        }
        m_typeAgent = newTypeAgent;
        switch(newTypeAgent)
        {
            case TypeAgent.Melee:
                m_life = 150f;
                m_armor = 30f;
                m_maxSteeringForce = 1f;
                m_maxVel = 6f;
                m_strength = 5f;
                m_eyesPerceptionRad = 5f;
                break;
            case TypeAgent.Shooter:
                m_life = 100f;
                m_armor = 10f;
                m_maxSteeringForce = 1f;
                m_maxVel = 7f;
                m_strength = 10f;
                m_eyesPerceptionRad = 15;
                break;
            case TypeAgent.Tank:
                m_life = 200f;
                m_armor = 60f;
                m_maxSteeringForce = 1f;
                m_maxVel = 5f;
                m_strength = 3f;
                m_eyesPerceptionRad = 10f;
                break;

        }
    }
    public float GetLife()
    {
        return m_life;
    }
    public void SetLife(float value)
    {
        m_life = value;
    }
    public float GetArmor()
    {
        return m_armor;
    }
    public void SetArmor(float value)
    {
        m_armor = value;
    }
    public float GetStrenght()
    {
        return m_strength;
    }
    public void SetStrenght(float value)
    {
        m_strength = value;
    }
    public TeamAgent GetTeamAgent()
    {
        return m_teamAgent;
    }

    public void SetTeamAgent(TeamAgent teamAgent)
    {
        if(m_teamAgent == teamAgent)
        {
            return;
        }
        m_teamAgent = teamAgent;
    }
    public bool GetTargetBool()
    {
        return m_justOneTarget;
    }
    public void SetTargetBool(bool value)
    {
        m_justOneTarget = value;
    }
    public float GetEyesRad()
    {
        return m_eyesPerceptionRad;
    }
}

public enum AgentState {
    None,
    Seeking,
    Fleing,
    Wandering
}
public enum TypeAgent
{
    Melee,
    Shooter,
    Tank
}

public enum TeamAgent
{
    Verde,
    Rojo
}
