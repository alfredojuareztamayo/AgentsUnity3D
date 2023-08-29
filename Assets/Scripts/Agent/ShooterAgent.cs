using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class ShooterAgent : BasicAgent {
    SteeringBehaviours m_sb;
    Rigidbody m_rigidbody;
    Collider[] eyesPerceived;
    BasicTarget m_basicTarget;
    float masa;
    bool llama;
    void Start() {
        m_sb = new SteeringBehaviours();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        perceptionManager();
        decisionManager();


        // Debug.Log(eyesPerceived.Length);

    }
    private void FixedUpdate() {
        eyesPerceived = Physics.OverlapSphere(m_eyesPerceptionPos.position, m_eyesPerceptionRad);
        
        //OnDrawGizmos();
    }
    private void perceptionManager() {
        if (m_target == null && eyesPerceived == null) {
            return;
        }
        foreach (Collider col in eyesPerceived) {
            if (col.CompareTag("Enemy")) {
                masa = col.GetComponent<BasicTarget>().peso;
                llama = col.GetComponent<BasicTarget>().tienesUnCinco;
                Debug.Log("traes un 5");
                if (!llama) {
                    m_target = col.transform;  
                    return;
                }
            }
        }
        eyesPerceived = null;
    }
    void decisionManager() {
        if(m_target == null) {
            changeAgentStaet(AgentState.Wandering);
            movementManager();
        }
        //ejecuta al movement manager, al action o ambos
    }
    void movementManager() {
       switch (getAgentState()) {
            case AgentState.None:
                break;
            case AgentState.Seeking:
                m_rigidbody.velocity = m_sb.seek(this.transform, m_target);
                break;
            case AgentState.Fleing:
                break;
            case AgentState.Wandering:
                break;
        }
    }
    void actionManager() {
        //weas como disparar, corar o asi mil.
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_eyesPerceptionPos.position, m_eyesPerceptionRad);
    }
    //private void OnTriggerEnter(Collider other) {
    //    if (other.CompareTag("Enemy")) {
    //        m_target = other.transform;
    //    }
    //}

    //private void OnTriggerExit(Collider other) {
    //    if (m_target != null) {
    //        m_target = null;
    //    }
    //}

}

