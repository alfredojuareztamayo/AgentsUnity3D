using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Class representing an agent that follows a predefined path.
/// </summary>
public class AgentFollower : MonoBehaviour
{
    [SerializeField]private Vector3[] path;
    Rigidbody m_rb;
    private int currentPath;
    void Start()
    {
       m_rb = GetComponent<Rigidbody>();
        currentPath = 0;
        path = new Vector3[5];
        path[0] = new Vector3(15,1,0);
        path[1] = new Vector3(15,1,-10);
        path[2] = new Vector3(30,1,-10);
        path[3] = new Vector3(30,1,5);
        path[4] = new Vector3(0,1,0);
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, path[currentPath]) < 1f)
        {
            // Avanzar al siguiente punto en la trayectoria
            currentPath++;

            // Comprobar si hemos llegado al final de la trayectoria
            if (currentPath >= path.Length)
            {
                // Reiniciar la trayectoria desde el principio (puedes ajustar esto según tus necesidades)
                currentPath = 0;
            }
        }
        transform.Translate(SteeringBehaviour2.PathFollow(path,transform,currentPath));
    }
}
