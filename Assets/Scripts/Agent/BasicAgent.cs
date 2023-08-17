using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicAgent : MonoBehaviour {

    [SerializeField] float maxVel, maxSteeringForce;
    SteeringBehaviours sb;
    Transform target;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        sb.seek(transform, target);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(target != null) {
            target = null;
        }
    }

    public float getMaxVel() {
        return maxVel;
    }
    public float getMaxSteeringForce() {
        return maxVel;
    }
}
