using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class ShooterAgent : BasicAgent {
    SteeringBehaviours m_sb;
    Rigidbody m_rigidbody;
    Collider[] eyesPerceived;

    void Start() {
        m_sb = new SteeringBehaviours();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (m_target != null) {
            m_rigidbody.velocity = m_sb.seek(this.transform, m_target);
            // transform.LookAt(m_target.position);
        }
    }
    private void FixedUpdate() {
        eyesPerceived = Physics.OverlapSphere(m_eyesPerceptionPos.position, m_eyesPerceptionRad);

    }
    private void perceptionManager() {
        if (m_target == null || eyesPerceived == null) {
            return;
        }
        foreach (Collider col in eyesPerceived) {
            if (col.CompareTag("Enemy")) {
               
            }
        }
        eyesPerceived = null;
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

