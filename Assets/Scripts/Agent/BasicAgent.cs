using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicAgent : MonoBehaviour {

    [SerializeField] float m_maxVel, m_maxSteeringForce;
    [SerializeField] protected float m_eyesPerceptionRad, m_earPerceptionRad;
    [SerializeField] protected Transform m_eyesPerceptionPos, m_earPerceptionPos;
    protected Transform m_target;

    public float getMaxVel() {
        return m_maxVel;
    }
    public float getMaxSteeringForce() {
        return m_maxVel;
    }
    //public Transform getTarget() {
    //    return m_target;
    //}
    //public void SetTarget(Transform target) {
    //    m_target = target;
    //}
}
