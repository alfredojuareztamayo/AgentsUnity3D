using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float velocidad = 10f; // Velocidad del misil en unidades por segundo.
    public float Da�o = 10f;
    private void Start()
    {
        // Establece la velocidad inicial del misil en la direcci�n hacia adelante.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * velocidad;

        // Destruye el misil despu�s de 3 segundos.
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        
    }
}
