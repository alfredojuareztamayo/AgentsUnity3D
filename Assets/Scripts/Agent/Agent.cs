using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent {
    float m_maxVel, m_maxSteeringForce;
    float m_life, m_armor, m_strength;
    float m_eyesPerceptionRad, m_earPerceptionRad;
    Transform m_eyesPerceptionPos, m_earPerceptionPos;
    Transform m_target;
    AgentStates m_agentState;
    TypeAgents m_typeAgent;
    TeamAgents m_teamAgent;

    public Agent() { }
    public Agent(float maxVel, float maxSteeringForce, float life, float armor, float strength, float eyesPerceptionRad, float earPerceptionRad, Transform eyesPerceptionPos, Transform earPerceptionPos, Transform target, AgentStates agentState, TypeAgents typeAgent, TeamAgents teamAgent)
    {
        m_maxVel = maxVel;
        m_maxSteeringForce = maxSteeringForce;
        m_life = life;
        m_armor = armor;
        m_strength = strength;
        m_eyesPerceptionRad = eyesPerceptionRad;
        m_earPerceptionRad = earPerceptionRad;
        m_eyesPerceptionPos = eyesPerceptionPos;
        m_earPerceptionPos = earPerceptionPos;
        m_target = target;
        m_agentState = agentState;
        m_typeAgent = typeAgent;
        m_teamAgent = teamAgent;
    }

    public float GetMaxVel()
    {
        return m_maxVel;
    }
    public float GetMaxSteeringForce()
    {
        return m_maxVel;
    }
    public void ChangeAgentStaet(AgentStates newState)
    {
        if (newState == m_agentState)
        {
            return;
        }
        m_agentState = newState;
        switch (newState)
        {
            case AgentStates.None:
                break;
            case AgentStates.Seeking:
                break;
            case AgentStates.Fleing:
                break;
            case AgentStates.Wandering:
                break;

        }
    }
    public AgentStates GetAgentState()
    {
        return m_agentState;
    }

    public TypeAgents GetTypeAgent()
    {
        return m_typeAgent;
    }
    public void SetTypeAgent(TypeAgents newTypeAgent)
    {
        if (m_typeAgent == newTypeAgent)
        {
            return;
        }
        m_typeAgent = newTypeAgent;
        switch (newTypeAgent)
        {
            case TypeAgents.Melee:
                m_life = 150f;
                m_armor = 30f;
                m_maxSteeringForce = 1f;
                m_maxVel = 6f;
                m_strength = 5f;
                m_eyesPerceptionRad = 5f;
                break;
            case TypeAgents.Shooter:
                m_life = 100f;
                m_armor = 10f;
                m_maxSteeringForce = 1f;
                m_maxVel = 7f;
                m_strength = 10f;
                m_eyesPerceptionRad = 15;
                break;
            case TypeAgents.Tank:
                m_life = 200f;
                m_armor = 60f;
                m_maxSteeringForce = 1f;
                m_maxVel = 5f;
                m_strength = 3f;
                m_eyesPerceptionRad = 10f;
                break;

        }
    }
    public TeamAgents GetTeamAgent()
    {
        return m_teamAgent;
    }

    public void SetTeamAgent(TeamAgents teamAgent)
    {
        if (m_teamAgent == teamAgent)
        {
            return;
        }
        m_teamAgent = teamAgent;
    }
}
public enum AgentStates
{
    None,
    Seeking,
    Fleing,
    Wandering
}
public enum TypeAgents
{
    Melee,
    Shooter,
    Tank
}

public enum TeamAgents
{
    Verde,
    Rojo
}

