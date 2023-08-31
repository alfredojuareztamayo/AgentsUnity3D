using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WalkingAgent : BasicAgent
{
    SteeringBehaviours m_sb;
    Rigidbody m_rigidbody;
    Collider[] eyesPerceived;
    private List<Collider> m_targetsList;
    private List<Collider> m_alliesList;
    private bool enterInEyesPerceived;
    void Start()
    {
        m_sb = new SteeringBehaviours();
        m_rigidbody = GetComponent<Rigidbody>();
        m_targetsList = new List<Collider>();
        m_alliesList = new List<Collider>();
        SetTypeAgent(TypeAgent.Tank);
        SetTargetBool(false);
        enterInEyesPerceived = true;

    }

    // Update is called once per frame
    void Update()
    {
        PerceptionManager();
        DecisionManager();


       //  Debug.Log(m_eyesPerceptionRad);

    }
    private void FixedUpdate()
    {
        eyesPerceived = Physics.OverlapSphere(m_eyesPerceptionPos.position, m_eyesPerceptionRad);

        //OnDrawGizmos();
    }
    private void PerceptionManager()
    {
        if (m_target == null && eyesPerceived == null)
        {
            return;
        }

        foreach (Collider col in eyesPerceived)
        {
            if (col.CompareTag("Enemy"))
            {
                if (enterInEyesPerceived)
                {
                    m_target = col.transform;
                    enterInEyesPerceived = false;
                    eyesPerceived = null;
                    break;
                }

            }
        }
      //  Debug.Log("Llegue aqui");
    }
    void DecisionManager()
    {
        if (m_target == null)
        {
            return;
            //ChangeAgentStaet(AgentState.None);
            //MovementManager();
        }
        ChangeAgentStaet(AgentState.Seeking);
        MovementManager();
        // MovementManager();

        //ejecuta al movement manager, al action o ambos
    }
    void MovementManager()
    {
        switch (GetAgentState())
        {
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
    void ActionManager()
    {
        //weas como disparar, corar o asi mil.
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_eyesPerceptionPos.position, m_eyesPerceptionRad);
    }
}
