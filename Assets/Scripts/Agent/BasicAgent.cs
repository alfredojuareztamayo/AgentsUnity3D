using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicAgent : MonoBehaviour {

    [SerializeField] float m_maxVel, m_maxSteeringForce;
    SteeringBehaviours m_sb;
    Transform m_target;
    Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start() {
        m_sb = new SteeringBehaviours();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if(m_target != null) {
            m_rigidbody.velocity = m_sb.seek(this.transform, m_target);
           // transform.LookAt(m_target.position);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            m_target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(m_target != null) {
            m_target = null;
        }
    }

    private void perceptionManager() {

    }
    public float getMaxVel() {
        return m_maxVel;
    }
    public float getMaxSteeringForce() {
        return m_maxVel;
    }
}
