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
    Rigidbody m_rigidbody;
    private StatsBasicAgent statsBA;
    private int currentPath;
    void Start()
    {
        InitializeComponents();
        //m_rigidbody = GetComponent<Rigidbody>();
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
                // Reiniciar la trayectoria desde el principio 
                currentPath = 0;
            }
        }
        m_rigidbody.velocity = SteeringBehaviour2.Seek(transform, path[currentPath]);
        //transform.Translate(SteeringBehaviour2.PathFollow(path,transform,currentPath));
    }
    private void InitializeComponents()
    {
        statsBA = GetComponent<StatsBasicAgent>();
        //m_stb = new SteeringBehaviour2();
        m_rigidbody = GetComponent<Rigidbody>();
        AgentBasic2 agentVigila = new AgentBasic2(TypeAgent2.Vigila);

        statsBA.Vida = agentVigila.Vida; // Valor predeterminado de vida
        statsBA.Velocidad = agentVigila.Velocidad; // Velocidad predeterminada
        statsBA.Armadura = agentVigila.Armadura; // Armadura predeterminada
        statsBA.Fuerza = agentVigila.Fuerza; // Fuerza predeterminada
        statsBA.MaxVel = agentVigila.MaxVel; // Velocidad máxima predeterminada
        statsBA.SteeringForce = agentVigila.SteeringForce; // Fuerza de dirección predeterminada
        statsBA.EyesPerceptionRad = 10f;
        statsBA.ArrivalRadius = 3.5f;
        statsBA.TypeAgent = TypeAgent2.Vigila;
        statsBA.WanderAngleDelta = 2f;
        // Establecer la máscara de capa según el equipo
       // layerMask = checkLayerTeam ? 1 << 6 : 1 << 7;
    }
}
