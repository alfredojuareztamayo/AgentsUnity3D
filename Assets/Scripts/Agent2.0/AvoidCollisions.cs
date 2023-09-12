using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidCollisions : MonoBehaviour
{
    public float avoidRadius = 2.0f; // Radio de detecci�n de obst�culos
    private LayerMask obstacleLayer; // Capa de obst�culos
    public float speed = 5.0f; // Velocidad de movimiento del agente

    private void Start()
    {
        obstacleLayer = 1 >> 8;
    }
    void Update()
    {
        // Calcular una direcci�n de movimiento predeterminada
        Vector3 moveDirection = transform.forward;

        // Detectar obst�culos en la direcci�n actual
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, avoidRadius, transform.forward, out hit, avoidRadius, obstacleLayer))
        {
            // Hay un obst�culo en el camino, calcula una nueva direcci�n evitando el obst�culo
            Vector3 avoidDirection = Vector3.Cross(Vector3.up, hit.normal);
            moveDirection = Quaternion.Euler(0, 90, 0) * avoidDirection; // Girar 90 grados respecto a la normal del obst�culo
        }

        // Moverse en la direcci�n calculada
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
