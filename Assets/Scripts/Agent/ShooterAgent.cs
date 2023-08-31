using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class ShooterAgent : BasicAgent {
    SteeringBehaviours m_sb;
    Rigidbody m_rigidbody;
    Collider[] eyesPerceived;
    private List<Collider> m_targetsList;
    private List<Collider> m_alliesList;
   
    void Start() {
        m_sb = new SteeringBehaviours();
        m_rigidbody = GetComponent<Rigidbody>();
        m_targetsList = new List<Collider>();
        m_alliesList = new List<Collider>();
        SetTypeAgent(TypeAgent.Shooter);
        SetTargetBool(false);


    }

    // Update is called once per frame
    void Update() {
        PerceptionManager();
        DecisionManager();


        // Debug.Log(eyesPerceived.Length);

    }
    private void FixedUpdate() {
        eyesPerceived = Physics.OverlapSphere(m_eyesPerceptionPos.position, m_eyesPerceptionRad);
        
        //OnDrawGizmos();
    }
    private void PerceptionManager() {
        if (m_target == null && eyesPerceived == null) {
            return;
        }
        //foreach (Collider col in eyesPerceived) {
        //    if (col.CompareTag("Enemy")) {
        //        m_targetsList.Add(col);
        //        SetTargetBool(true);
        //    }
        //    if (col.CompareTag("Allie")){
        //        m_alliesList.Add(col);
        //    }
        //}
        eyesPerceived = null;
       
    }
    void DecisionManager() {

        float vida = m_life;
        float vidaToCompare;

        foreach (Collider col in m_targetsList)
        {
            vidaToCompare = col.GetComponent<ShooterAgent>().m_life;
           // Debug.Log("La vida del enemigo es: " + vidaToCompare);
            if (vida <= vidaToCompare)
            {
                m_target = col.transform;
               // Debug.Log("Encontre al enemigo");
                break;
            }
        }
        //if(m_target == null) {
        //    ChangeAgentStaet(AgentState.None);
        //    MovementManager();
        //}
        if (m_target != null)
        {
            ChangeAgentStaet(AgentState.Seeking);
            MovementManager();
        }
        
        //ejecuta al movement manager, al action o ambos
    }
    void MovementManager() {
       switch (GetAgentState()) {
            case AgentState.None:
                break;
            case AgentState.Seeking:
                m_rigidbody.velocity = m_sb.Seek(this.transform, m_target);
                break;
            case AgentState.Fleing:
                m_rigidbody.velocity = m_sb.Flee(this.transform, m_target);
                break;
            case AgentState.Wandering:
               // m_rigidbody.velocity = m_sb.Wander(this.transform);
                break;
        }
    }
    void ActionManager() {
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

