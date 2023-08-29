using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicAgent : MonoBehaviour {

    [SerializeField] float m_maxVel, m_maxSteeringForce;
    [SerializeField] protected float m_eyesPerceptionRad, m_earPerceptionRad;
    [SerializeField] protected Transform m_eyesPerceptionPos, m_earPerceptionPos;
    protected Transform m_target;
    private AgentState m_agentState;
    public float getMaxVel() {
        return m_maxVel;
    }
    public float getMaxSteeringForce() {
        return m_maxVel;
    }
   public void changeAgentStaet(AgentState newState) {
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
    public AgentState getAgentState() {
        return m_agentState;
    }
}

public enum AgentState {
    None,
    Seeking,
    Fleing,
    Wandering
}
