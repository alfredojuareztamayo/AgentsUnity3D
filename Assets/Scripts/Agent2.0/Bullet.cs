using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class represents a projectile that is fired and destroyed upon colliding with other objects.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    /// <summary>
    /// Speed of the projectile in units per second.
    /// </summary>
    public float velocidad = 10f;

    /// <summary>
    /// Damage inflicted by this projectile when colliding with other objects.
    /// </summary>
    public float Daño = 10f;

    private void Start() {
        // Set the initial velocity of the projectile in the forward direction.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * velocidad;

        // Destroy the projectile after 3 seconds.
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other) {
        // Destroy the projectile upon colliding with another object.
        Destroy(gameObject);
    }
}

