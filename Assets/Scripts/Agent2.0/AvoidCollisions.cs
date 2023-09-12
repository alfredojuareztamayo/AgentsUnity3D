using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidCollisions : MonoBehaviour
{
    public float avoidRadius = 2.0f; // Radio de detección de obstáculos
    private LayerMask obstacleLayer; // Capa de obstáculos
    public float speed = 5.0f; // Velocidad de movimiento del agente

    private void Start()
    {
        obstacleLayer = 1 >> 8;
    }
    void Update()
    {
        // Calcular una dirección de movimiento predeterminada
        Vector3 moveDirection = transform.forward;

        // Detectar obstáculos en la dirección actual
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, avoidRadius, transform.forward, out hit, avoidRadius, obstacleLayer))
        {
            // Hay un obstáculo en el camino, calcula una nueva dirección evitando el obstáculo
            Vector3 avoidDirection = Vector3.Cross(Vector3.up, hit.normal);
            moveDirection = Quaternion.Euler(0, 90, 0) * avoidDirection; // Girar 90 grados respecto a la normal del obstáculo
        }

        // Moverse en la dirección calculada
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
