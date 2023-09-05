using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWander : MonoBehaviour
{
    public float speed = 5.0f; // Velocidad de movimiento
    public float rotationSpeed = 2.0f; // Velocidad de rotación
    private Vector3 wanderTarget;

    void Start()
    {
        // Inicializar la posición de vagabundeo al comienzo
        SetNewWanderTarget();
    }

    void Update()
    {
        // Calcular la dirección hacia el objetivo de vagabundeo
        Vector3 direction = wanderTarget - transform.position;
        direction.Normalize();

        // Rotar hacia la dirección de vagabundeo
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

        // Moverse hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Si estamos cerca del objetivo de vagabundeo, elige un nuevo objetivo
        if (Vector3.Distance(transform.position, wanderTarget) < 1.0f)
        {
            SetNewWanderTarget();
        }
    }

    void SetNewWanderTarget()
    {
        // Generar un punto aleatorio dentro de un cierto radio
        float wanderRadius = 10.0f;
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * wanderRadius;

        // Establecer la altura al mismo nivel que el objeto
        randomDirection.y = transform.position.y;

        // Sumar el punto aleatorio al la posición actual para obtener el objetivo de vagabundeo
        wanderTarget = transform.position + randomDirection;
    }
}
